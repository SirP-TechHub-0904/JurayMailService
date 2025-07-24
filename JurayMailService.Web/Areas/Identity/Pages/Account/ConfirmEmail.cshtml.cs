using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Commands.WalletCommands;
using Application.DTO;
using Application.Queries.DashboardQueries;
using Domain.Models;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace JurayMailService.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator _mediator;
        private readonly AppDBContext _dbContext;

        public ConfirmEmailModel(UserManager<AppUser> userManager, IMediator mediator, AppDBContext dbContext)
        {
            _userManager = userManager;
            _mediator = mediator;
            _dbContext = dbContext;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                AddWalletCommand walletandSubUpdate = new AddWalletCommand(userId);
                await _mediator.Send(walletandSubUpdate);
                StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

                return RedirectToPage("/Account/Configuration", new {area="User"});

            }
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
        }
    }
}
