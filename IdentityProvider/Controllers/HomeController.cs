using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IdentityProvider.Models;
using Shared.UtconnectIdentity.Services;

namespace IdentityProvider.Controllers;

public class HomeController : Controller
{
    private readonly IIdentityService _identityService;

    public HomeController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public IActionResult Index()
    {
        var x = _identityService.GetCurrent();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}