using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sso.Presentation.Pages;

public class AuthenticateModel : PageModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public string? ReturnUrl { get; set; }
    public string AuthStatus { get; set; } = "";

    public IActionResult OnGet(string returnUrl)
    {
        ReturnUrl = returnUrl;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string email, string password)
    {
        if (IsAuthenticated(email, password))
        {
            AuthStatus = "Email or password is invalid";
            return Page();
        }

        await SignIn(email);

        if (!string.IsNullOrEmpty(ReturnUrl))
        {
            return Redirect(ReturnUrl);
        }

        AuthStatus = "Successfully authenticated";
        return Page();
    }

    private bool IsAuthenticated(string email, string password)
    {
        return email != "sysadmin" || password != "sysadmin";
    }

    private async Task SignIn(string email)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, email)
        };
        
        var identities = new List<ClaimsIdentity> { new(claims, CookieAuthenticationDefaults.AuthenticationScheme) };
        ClaimsPrincipal principal = new(identities);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}