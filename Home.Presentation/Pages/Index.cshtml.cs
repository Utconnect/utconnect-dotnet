using Microsoft.AspNetCore.Mvc.RazorPages;
using Oidc.Domain.Models;

namespace Home.Presentation.Pages;

public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
    public ExchangeTokenResponse? ExchangeTokenResponse { get; private set; }

    public PageResult OnGet(ExchangeTokenResponse tokenResponse)
    {
        if (string.IsNullOrEmpty(tokenResponse.AccessToken) ||
            string.IsNullOrEmpty(tokenResponse.RefreshToken) ||
            tokenResponse is not { TokenType: "Bearer", ExpiresIn: > 0 })
        {
            return Page();
        }

        logger.LogInformation("Redirected from login site");
        ExchangeTokenResponse = tokenResponse;

        return Page();
    }
}