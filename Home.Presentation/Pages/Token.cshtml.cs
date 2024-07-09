using Home.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Services.Abstractions;

namespace Home.Presentation.Pages;

public class TokenModel(IDateTime dateTime, ILogger<TokenModel> logger) : PageModel
{
    [FromQuery(Name = "access_token")]
    public string AccessToken { get; set; } = null!;

    [FromQuery(Name = "token_type")]
    public string TokenType { get; set; } = null!;

    [FromQuery(Name = "expires_in")]
    public int ExpiresIn { get; set; }

    [FromQuery(Name = "refresh_token")]
    public string RefreshToken { get; set; } = null!;

    public ActionResult OnGet()
    {
        if (string.IsNullOrEmpty(AccessToken) ||
            string.IsNullOrEmpty(RefreshToken) ||
            TokenType != "Bearer" ||
            ExpiresIn <= 0)
        {
            ClearCookies();
            return RedirectToPage("/Index");
        }

        logger.LogInformation("Redirected from login site");
        SetCookies();

        return RedirectToPage("/Index");
    }

    private void ClearCookies()
    {
        Response.Cookies.Delete(TokenConstants.AccessToken);
    }

    private void SetCookies()
    {
        Response.Cookies.Append(TokenConstants.AccessToken, AccessToken, new CookieOptions
        {
            Expires = dateTime.Now.AddSeconds(ExpiresIn)
        });
    }
}