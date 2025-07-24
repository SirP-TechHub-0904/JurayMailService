using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Plan
    {
        public long Id { get; set; }
        public string? Name { get; set; } // e.g., "Free", "Basic", "Pro"
        public int MonthlyLimit { get; set; } // How many emails the user can submit per month
        public decimal Price { get; set; } // Price per month
    }
}
