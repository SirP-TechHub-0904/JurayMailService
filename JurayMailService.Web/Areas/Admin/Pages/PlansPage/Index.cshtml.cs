using Application.Queries.PlanQueries;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayMailService.Web.Areas.Admin.Pages.PlansPage
{
    [Authorize(Roles = "Admin")]

    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IList<Plan> Plans { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            ListAllPlanQuery Command = new ListAllPlanQuery();
            Plans = await _mediator.Send(Command);

            return Page();
        }
    }
}
