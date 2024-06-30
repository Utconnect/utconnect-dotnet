using Microsoft.AspNetCore.Mvc;

namespace Sso.Presentation.Controllers;

public class ExchangeController(ILogger<ExchangeController> logger) : Controller
{
    public IActionResult Index(string? returnUrl)
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            logger.LogInformation("Authenticated. Getting token");
            // Get token
        }
        else
        {
            logger.LogInformation("Unauthenticated. Navigating to /login");
            return RedirectToAction("Index", "Login", new { ReturnUrl = returnUrl });
        }

        return View(returnUrl);
    }
}