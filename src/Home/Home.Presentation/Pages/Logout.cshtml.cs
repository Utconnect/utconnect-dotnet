using Home.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Home.Presentation.Pages;

public class LogoutModel(ILogger<LogoutModel> logger) : PageModel
{
    public ActionResult OnGet()
    {
        logger.LogInformation("Logout");
        Response.Cookies.Delete(TokenConstants.AccessToken);

        return RedirectToPage("/Index");
    }
}