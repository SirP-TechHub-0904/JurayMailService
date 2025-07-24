using Application.Commands.EmailProjectCommands;
using Application.Commands.PlanCommands;
using Application.Queries.EmailProjectQueries;
using Application.Queries.PlanQueries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayMailService.Web.Areas.Admin.Pages.PlansPage
{
     [Authorize]
    public class UpdateModel : PageModel
    {
        private readonly IMediator _mediator;

        public UpdateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public Plan Plan { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            if (id < 0)
            {
                return NotFound();
            }
            GetByIdPlanQuery Command = new GetByIdPlanQuery(id);
            Plan = await _mediator.Send(Command);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                UpdatePlanCommand Command = new UpdatePlanCommand(Plan);
                await _mediator.Send(Command);
                TempData["success"] = "Success";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                return Page();

            }
        }
    }
}
