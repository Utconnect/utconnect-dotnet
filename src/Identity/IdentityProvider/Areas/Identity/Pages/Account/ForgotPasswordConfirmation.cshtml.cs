using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProvider.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ForgotPasswordConfirmation(RedirectService redirectService) : PageModel
{
    public IActionResult OnGet()
    {
        return redirectService.GetResultForPublicOnlyPage(User, Redirect, Page);
    }
}