using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Plan
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } // e.g., Free Plan, Pro Plan, Premium Plan

        [Required]
        public decimal Price { get; set; } // e.g., 0.00, 2000.00, 4500.00

       public int MonthlyLimit { get; set; } // e.g., 500, 2000, 5000

        public virtual ICollection<PlanFeature> Features { get; set; } = new List<PlanFeature>();
    }
    public class PlanFeature
    {
        public long Id { get; set; }

        public long PlanId { get; set; }

        [Required]
        public string Description { get; set; } // e.g., "500 Emails Monthly", "Custom SMTP Allowed"

        public bool IsHighlighted { get; set; } = true; // For <strong> tags if needed

        public virtual Plan Plan { get; set; }
    }

}
