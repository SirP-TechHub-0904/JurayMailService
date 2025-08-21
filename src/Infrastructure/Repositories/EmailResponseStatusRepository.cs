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
    public sealed class EmailResponseStatusRepository : Repository<EmailResponseStatus>, IEmailResponseStatusRepository
    {
        private readonly AppDBContext _context;

        public EmailResponseStatusRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<EmailResponseStatus>> GetAllResponseByMessageId(string messageId)
        {
           var list = await _context.EmailResponseStatuses 
                .Where(x=>x.MessageId == messageId).ToListAsync();

            return list;
        }

        public async Task<IEnumerable<EmailResponseStatus>> GetResponseListByUserIdAsync(int pageNumber, int pageSize, string userId)
        {
            var list = _context.EmailResponseStatuses
                .Include(x => x.EmailProject)
                .Where(x => x.UserId == userId)
                .OrderByDescending(e => e.SentDate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);
            return await list.ToListAsync();
        }

        public async Task<List<MonthlyEmailStatsDto>> GetMonthlyStatsByUserIdAsync(string userId)
        {
            int currentYear = DateTime.UtcNow.Year; // Set default to current year

            // Fetch Total Sent Emails from EmailSendingStatuses
            var sentData = await _context.EmailSendingStatuses
                .Where(x => x.UserId == userId && x.SubmittedDate.Year == currentYear)
                .GroupBy(x => x.SubmittedDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalSent = g.Count() // Count emails per month
                })
                .ToListAsync();

            // Fetch Delivered & Open Data from EmailResponseStatus
            var responseData = await _context.EmailResponseStatuses
                .Where(x => x.UserId == userId && x.SentDate.HasValue && x.SentDate.Value.Year == currentYear)
                .GroupBy(x => x.SentDate.Value.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalDelivered = g.Count(x => x.RecordType == "Delivery"),
                    TotalReceived = g.Count(x => x.RecordType == "Open")
                })
                .ToListAsync();

            // Merge Data from Both Queries
            var monthlyStats = new List<MonthlyEmailStatsDto>();
            for (int month = 1; month <= 12; month++)
            {
                var sentRecord = sentData.FirstOrDefault(s => s.Month == month);
                var responseRecord = responseData.FirstOrDefault(r => r.Month == month);

                monthlyStats.Add(new MonthlyEmailStatsDto
                {
                    Month = month,
                    TotalSent = sentRecord?.TotalSent ?? 0, // Correct Total Sent
                    TotalDelivered = responseRecord?.TotalDelivered ?? 0,
                    TotalReceived = responseRecord?.TotalReceived ?? 0
                });
            }

            return monthlyStats;
        }

    }
}
