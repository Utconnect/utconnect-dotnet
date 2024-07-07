using Home.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oidc.Domain.Models;
using Shared.Services.Abstractions;

namespace Home.Presentation.Pages;

public class TokenModel(IDateTime dateTime, ILogger<TokenModel> logger) : PageModel
{
    public ActionResult OnGet(ExchangeTokenResponse tokenResponse)
    {
        if (string.IsNullOrEmpty(tokenResponse.AccessToken) ||
            string.IsNullOrEmpty(tokenResponse.RefreshToken) ||
            tokenResponse is not { TokenType: "Bearer", ExpiresIn: > 0 })
        {
            ClearCookies();
            return RedirectToPage("/");
        }

        logger.LogInformation("Redirected from login site");
        SetCookies(tokenResponse);

        return RedirectToPage("/");
    }

    private void ClearCookies()
    {
        Response.Cookies.Delete(TokenConstants.AccessToken);
        Response.Cookies.Delete(TokenConstants.RefreshToken);
    }

    private void SetCookies(ExchangeTokenResponse tokenResponse)
    {
        Response.Cookies.Append(TokenConstants.AccessToken, tokenResponse.AccessToken, new CookieOptions
        {
            Expires = dateTime.Now.AddSeconds(tokenResponse.ExpiresIn)
        });
        Response.Cookies.Append(TokenConstants.RefreshToken, tokenResponse.RefreshToken);
    }
}