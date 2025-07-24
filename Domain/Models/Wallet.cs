using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Foreign key to Users table
        public AppUser User { get; set; }

        public decimal Balance { get; set; } // Available balance
    }
}
