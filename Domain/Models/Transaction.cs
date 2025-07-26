using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enum.EnumStatus;

namespace Domain.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Foreign key to Users table
        public AppUser User { get; set; }

        public decimal Amount { get; set; } // Positive for deposits, negative for deductions
        public TransactionType Type { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string Reference { get; set; } // e.g., Paystack or Wallet reference

        public string Description { get; set; }
    }
}
