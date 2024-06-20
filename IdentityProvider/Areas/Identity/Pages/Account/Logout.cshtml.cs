using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProvider.Areas.Identity.Pages.Account;

public class LogoutModel(ILogger<LogoutModel> logger) : PageModel
{
    public async Task<IActionResult> OnPost(string? returnUrl = null)
    {
        await HttpContext.SignOutAsync();
        logger.LogInformation("User logged out");
        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }

        // This needs to be a redirect so that the browser performs a new
        // request and the identity for the user gets updated.
        return RedirectToPage();
    }
}