using Microsoft.AspNetCore.Mvc;

namespace Sso.Presentation.Controllers;

public class LoginController(ILogger<LoginController> logger) : Controller
{
    public IActionResult Index(string? returnUrl)
    {
        return View(returnUrl);
    }
}