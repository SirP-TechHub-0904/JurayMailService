using Application.Queries.EmailProjectQueries;
using Application.Queries.PlanQueries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace JurayMailService.Web.Areas.Admin.Pages.PlansPage
{
    [Authorize]
    public class InfoModel : PageModel
    {
        private readonly IMediator _mediator;
 
        public InfoModel(IMediator mediator)
        {
            _mediator = mediator;
         }

        [BindProperty]
        public Plan Plan { get; set; }

         
        public async Task<IActionResult> OnGetAsync(long id)
        {
            if (id == null)
            {
                return NotFound();
            }
            GetByIdPlanQuery Command = new GetByIdPlanQuery(id);
            Plan = await _mediator.Send(Command);
            return Page();
        }

     }

}
