using Application.Commands.EmailGroupCommands;
using Application.Commands.EmailListCommands;
using Application.Queries.EmailGroupQueries;
using Application.Queries.EmailListQueries;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace JurayMailService.Web.Areas.User.Pages.Groups
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
        public EmailGroup EmailGroup { get; set; }
        public List<EmailList> EmailLists { get; set; }

        [BindProperty]
        public IFormFile ExcelFile { get; set; }

        [BindProperty]
        public long GroupId { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            if (id < 0)
            {
                return NotFound();
            }
            GetByIdEmailGroupQuery Command = new GetByIdEmailGroupQuery(id);
            EmailGroup = await _mediator.Send(Command);


            ListByGroupIdEmailListQuery listcommand = new ListByGroupIdEmailListQuery(id);
            EmailLists = await _mediator.Send(listcommand);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                UploadEmailsToGroup Command = new UploadEmailsToGroup(ExcelFile, GroupId, userId);
                await _mediator.Send(Command);
                TempData["success"] = "Success";
                return RedirectToPage("./Info", new {id = GroupId});
            }
            catch (Exception ex)
            {
                 
                GetByIdEmailGroupQuery Command = new GetByIdEmailGroupQuery(GroupId);
                EmailGroup = await _mediator.Send(Command);


                ListByGroupIdEmailListQuery listcommand = new ListByGroupIdEmailListQuery(GroupId);
                EmailLists = await _mediator.Send(listcommand);
                return Page();

            }
        }


        [BindProperty] public long EditEmail_Id { get; set; }
        [BindProperty] public string EditEmail_Name { get; set; }
        [BindProperty] public string EditEmail_Email { get; set; }
        [BindProperty] public string EditEmail_PhoneNumber { get; set; }

        [BindProperty] public long DeleteEmail_Id { get; set; }

        // Edit handler
        public async Task<IActionResult> OnPostEditEmailAsync()
        {
            // 1) Load existing contact
            var existing = await _mediator.Send(new GetByIdEmailListQuery(EditEmail_Id));
            if (existing is null)
            {
                TempData["error"] = "Contact not found.";
                return RedirectToPage("./Info", new { id = GroupId });
            }

            // 2) Apply edits (trim + keep existing when empty)
            existing.Name = string.IsNullOrWhiteSpace(EditEmail_Name) ? existing.Name : EditEmail_Name.Trim();
            existing.Email = string.IsNullOrWhiteSpace(EditEmail_Email) ? existing.Email : EditEmail_Email.Trim();
            existing.PhoneNumber = string.IsNullOrWhiteSpace(EditEmail_PhoneNumber) ? existing.PhoneNumber : EditEmail_PhoneNumber.Trim();

            // Optional: basic email guard
            if (string.IsNullOrWhiteSpace(existing.Email))
            {
                TempData["error"] = "Email is required.";
                return RedirectToPage("./Info", new { id = GroupId });
            }

            // 3) Persist
            await _mediator.Send(new UpdateEmailListCommand(existing));

            TempData["success"] = "Contact updated.";
            return RedirectToPage("./Info", new { id = GroupId });
        }

        // Delete handler
        public async Task<IActionResult> OnPostDeleteEmailAsync()
        {
            await _mediator.Send(new DeleteEmailListCommand(DeleteEmail_Id));
            TempData["success"] = "Contact deleted.";
            return RedirectToPage("./Info", new { id = GroupId });
        }


    }

}
