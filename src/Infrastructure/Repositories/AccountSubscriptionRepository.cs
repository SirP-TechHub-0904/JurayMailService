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
  
    public sealed class AccountSubscriptionRepository : Repository<AccountSubscription>, IAccountSubscriptionRepository
    {
        private readonly AppDBContext _context;

        public AccountSubscriptionRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<AccountSubscription> GetByUserIdAsync(string userId)
        {
            return await _context.AccountSubscriptions.FirstOrDefaultAsync(w => w.UserId == userId);
        }
        public async Task<string> RenewSubscription(string userId)
        {
            var userSubscription = await _context.AccountSubscriptions
                .Include(us => us.Plan)
                .FirstOrDefaultAsync(us => us.UserId == userId);

            if (userSubscription == null) return "user not found"; // No subscription found

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null || wallet.Balance < userSubscription.Plan.Price)
                return "Insufficient funds"; // Insufficient funds

            // Deduct subscription fee
            wallet.Balance -= userSubscription.Plan.Price;

            _context.Transactions.Add(new Transaction
            {
                UserId = userId,
                Amount = -userSubscription.Plan.Price,
                Type = Domain.Enum.EnumStatus.TransactionType.Subscription
            });

            // ✅ Reset Monthly Email Quota
            userSubscription.RemainingEmailsForMonth = userSubscription.Plan.MonthlyLimit;

            // Extend subscription
            userSubscription.SubscriptionEndDate = DateTime.UtcNow.AddMonths(1);

            await _context.SaveChangesAsync();
            return "successful";
        }
        public async Task<(bool Success, string Message)> ProcessEmailSending(string userId)
        {
            var userSubscription = await _context.AccountSubscriptions
                .Include(us => us.Plan)
                .FirstOrDefaultAsync(us => us.UserId == userId);

            if (userSubscription == null)
                return (false, "No active subscription found.");

            if (userSubscription.RemainingEmailsForMonth <= 0)
                return (false, "You have reached your monthly email limit. Upgrade your plan or buy extra credits.");

            // ✅ Deduct one email from the quota
            userSubscription.RemainingEmailsForMonth -= 1;

            await _context.SaveChangesAsync();

            return (true, "Email quota updated successfully.");
        }

    }
}
