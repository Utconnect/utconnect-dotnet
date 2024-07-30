using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Shared.Application.Configuration.Models;

namespace IdentityProvider.Areas.Identity.Pages.Account;

public class LogoutModel(IOptions<HomeConfig> homeConfig, ILogger<LogoutModel> logger) : PageModel
{
    public async Task<IActionResult> OnPost()
    {
        await HttpContext.SignOutAsync();
        logger.LogInformation("User logged out");

        string logoutRedirectUrl = homeConfig.Value.Url + homeConfig.Value.LogoutPath;

        return Redirect(logoutRedirectUrl);
    }
}