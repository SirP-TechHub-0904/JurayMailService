using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserSubscription
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Foreign key to Users table
        public AppUser User { get; set; }
        public long PlanId { get; set; } // Foreign key to Plans table
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime SubscriptionEndDate { get; set; }

        public Plan Plan { get; set; }


        public int RemainingEmailsForMonth { get; set; }
    }

}
