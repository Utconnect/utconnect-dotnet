using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using IdentityProvider.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Shared.Infrastructure.Email.Services.Abstract;

namespace IdentityProvider.Areas.Identity.Pages.Account;

public class ForgotPasswordModel(
    UserManager<User> userManager,
    IEmailService emailService,
    IStringLocalizer<I18NResource> localizer,
    ILogger<LoginModel> logger
)
    : PageModel
{
    [BindProperty]
    public InputModel? Input { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; init; }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid || Input?.Email == null)
        {
            return Page();
        }

        User? user = await userManager.FindByEmailAsync(Input.Email);
        if (user == null || !await userManager.IsEmailConfirmedAsync(user))
        {
            // Don't reveal that the user does not exist or is not confirmed
            logger.LogInformation("Email is not found or not confirmed");
            return RedirectToPage("./forgot-password/confirmation");
        }

        string code = await userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        string? callbackUrl = Url.Page(
            "/Account/ResetPassword",
            pageHandler: null,
            values: new { area = "Identity", code },
            protocol: Request.Scheme);

        if (callbackUrl == null)
        {
            return Page();
        }

        if (await emailService.SendResetPassword(Input.Email, HtmlEncoder.Default.Encode(callbackUrl),
            cancellationToken))
        {
            logger.LogInformation("Email reset password is sent");
            return RedirectToPage("./forgot-password/confirmation");
        }

        logger.LogInformation("Cannot send email reset password");
        ModelState.AddModelError(string.Empty, localizer["SystemErrorTryLater"]);
        return Page();
    }
}