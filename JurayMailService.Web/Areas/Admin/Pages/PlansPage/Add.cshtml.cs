using Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Application.Commands.ServerCommands;
using Application.Commands.PlanCommands;

namespace JurayMailService.Web.Areas.Admin.Pages.PlansPage
{
    [Authorize(Roles = "Admin")]
    public class AddModel : PageModel
    {
        private readonly IMediator _mediator;

        public AddModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public Plan Plan { get; set; }

        
        public async Task OnGetAsync()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                AddPlanCommand Command = new AddPlanCommand(Plan);
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
