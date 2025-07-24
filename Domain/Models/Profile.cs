using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Profile
    {
        public long Id { get; set; }


        [Display(Name = "Fullname")]
        public string? Fullname { get; set; }

        public string? UserId { get; set; }
        public AppUser User { get; set; }

        public string? BusinessName { get; set; }
        public bool DoYouHaveADomain { get; set; }
        public string? DomainName { get; set; }
        public string? Alias { get; set; }

        public long? PlanId { get; set; }
        public Plan Plan { get; set; }
    }
}
