using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    
    public class MonthlyEmailStatsDto
    {
        public int Month { get; set; } // 1=Jan, 12=Dec
        public int TotalSent { get; set; }
        public int TotalDelivered { get; set; }
        public int TotalReceived { get; set; }
    }
}
