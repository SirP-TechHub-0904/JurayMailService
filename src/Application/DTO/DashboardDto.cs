using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class DashboardDto
    {
        public int AllProjects {  get; set; }
        public int AllEmails { get; set; }


        // This Month Stats
        public int ThisMonthTotalProjects { get; set; }
        public int ThisMonthTotalSubmitted { get; set; }
        public int ThisMonthTotalDelivered { get; set; }
        public int ThisMonthTotalOpened { get; set; }

        // All Time Stats
        public int AllTimeTotalProjects { get; set; }
        public int AllTimeTotalSubmitted { get; set; }
        public int AllTimeTotalDelivered { get; set; }
        public int AllTimeTotalOpened { get; set; }

        // Statistics
        public int TotalEmailsInSystemWithoutDuplicate { get; set; }
        public decimal TotalAmountSpent { get; set; }
        public decimal Balance {  get; set; }
        public int Limit { get; set; }




        public List<MonthlyEmailStatsDto> MonthlyStats { get; set; } = new List<MonthlyEmailStatsDto>();



    }

   
}
