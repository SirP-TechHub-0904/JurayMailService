using Domain.GenericInterface;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAccountSubscriptionRepository : IRepository<AccountSubscription>
    {
        Task<string> RenewSubscription(string userId);
        Task<AccountSubscription> GetByUserIdAsync(string userId);

     }
}
