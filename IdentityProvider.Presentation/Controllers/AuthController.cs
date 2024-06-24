using IdentityProvider.Presentation.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProvider.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AuthController : Controller
{
    [HttpGet("validate-cookie")]
    [JsonAuthorization]
    public IActionResult ValidateCookie()
    {
        return Ok();
    }
}