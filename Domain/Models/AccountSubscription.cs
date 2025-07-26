using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AccountSubscription
    {
        public long Id { get; set; }
        public string UserId { get; set; } // Foreign key to Users table
        public AppUser User { get; set; }
        public long PlanId { get; set; } // Foreign key to Plans table
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public bool IsActive => DateTime.UtcNow >= SubscriptionStartDate && DateTime.UtcNow <= SubscriptionEndDate;
        public Plan Plan { get; set; }


        public int RemainingEmailsForMonth { get; set; }
    }

}
