using Home.Application.User.Queries.GetUserInfo;
using Home.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Home.Presentation.Pages;

public class IndexModel(IMediator mediator) : PageModel
{
    public async Task<ActionResult> OnGet()
    {
        string? accessToken = HttpContext.Request.Cookies[TokenConstants.AccessToken];
        string? refreshToken = HttpContext.Request.Cookies[TokenConstants.RefreshToken];

        if (!string.IsNullOrEmpty(accessToken))
        {
            var result = await mediator.Send(new GetUserInfoQuery(accessToken));
        }

        return Page();
    }
}