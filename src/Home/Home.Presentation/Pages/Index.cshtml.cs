using Home.Application.User.Queries.GetUserInfo;
using Home.Presentation.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oidc.Domain.Models;

namespace Home.Presentation.Pages;

public class IndexModel(IMediator mediator) : PageModel
{
    public async Task<ActionResult> OnGet()
    {
        string? accessToken = HttpContext.Request.Cookies[TokenConstants.AccessToken];

        if (string.IsNullOrEmpty(accessToken))
        {
            return Page();
        }

        UserInfoResponse? result = await mediator.Send(new GetUserInfoQuery(accessToken));
        if (result != null)
        {
            ViewData["UserName"] = result.UserName;
        }

        return Page();
    }
}