using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Context;
using Infrastructure.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
       public sealed class EmailProjectRepository : Repository<EmailProject>, IEmailProjectRepository
    {
        private readonly AppDBContext _context;

        public EmailProjectRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<EmailProject>> GetAllByUserId(string userId)
        {
            return await _context.EmailProjects.Where(x => x.AppUserId == userId).ToListAsync();

        }

        public async Task<int> GetProjectCountByUserId(string userId)
        {
            return await _context.EmailProjects.Where(x=>x.AppUserId == userId).CountAsync();
        }

        public async Task<DashboardModelDto> GetDashboardStatsByUserIdAsync(string userId)
        {
            int currentYear = DateTime.UtcNow.Year;
            int currentMonth = DateTime.UtcNow.Month;

            // Fetch "This Month" Sent Stats
            var thisMonthSentStats = await _context.EmailSendingStatuses
                .Where(x => x.UserId == userId && x.SubmittedDate.Month == currentMonth && x.SubmittedDate.Year == currentYear)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalSubmitted = g.Count()
                })
                .FirstOrDefaultAsync();

            // Fetch "This Month" Delivered & Opened Stats
            var thisMonthResponseStats = await _context.EmailResponseStatuses
                .Where(x => x.UserId == userId && x.SentDate.HasValue && x.SentDate.Value.Month == currentMonth && x.SentDate.Value.Year == currentYear)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalDelivered = g.Count(x => x.RecordType == "Delivery"),
                    TotalOpened = g.Count(x => x.RecordType == "Open")
                })
                .FirstOrDefaultAsync();

            // Fetch "All Time" Sent Stats
            var allTimeSentStats = await _context.EmailSendingStatuses
                .Where(x => x.UserId == userId)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalSubmitted = g.Count()
                })
                .FirstOrDefaultAsync();

            // Fetch "All Time" Delivered & Opened Stats
            var allTimeResponseStats = await _context.EmailResponseStatuses
                .Where(x => x.UserId == userId)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalDelivered = g.Count(x => x.RecordType == "Delivery"),
                    TotalOpened = g.Count(x => x.RecordType == "Open")
                })
                .FirstOrDefaultAsync();

            // Get unique email count
            int totalUniqueEmails = await _context.EmailLists
                .Select(x => x.Email)
                .Distinct()
                .CountAsync();
            // Get total amount spent
            decimal totalAmountSpent = await _context.EmailSendingStatuses.SumAsync(x => x.Retries);

            return new DashboardModelDto
            {
                // This Month
                ThisMonthTotalProjects = await _context.EmailProjects.CountAsync(x => x.AppUserId == userId && x.Date.Month == currentMonth && x.Date.Year == currentYear),
                ThisMonthTotalSubmitted = thisMonthSentStats?.TotalSubmitted ?? 0,
                ThisMonthTotalDelivered = thisMonthResponseStats?.TotalDelivered ?? 0,
                ThisMonthTotalOpened = thisMonthResponseStats?.TotalOpened ?? 0,

                // All Time
                AllTimeTotalProjects = await _context.EmailProjects.CountAsync(x => x.AppUserId == userId),
                AllTimeTotalSubmitted = allTimeSentStats?.TotalSubmitted ?? 0,
                AllTimeTotalDelivered = allTimeResponseStats?.TotalDelivered ?? 0,
                AllTimeTotalOpened = allTimeResponseStats?.TotalOpened ?? 0,

                // Statistics
                TotalEmailsInSystemWithoutDuplicate = totalUniqueEmails,
                TotalAmountSpent = totalAmountSpent
            };
        }

    }
}
