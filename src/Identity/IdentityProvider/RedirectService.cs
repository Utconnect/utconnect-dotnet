using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Shared.Application.Configuration.Models;

namespace IdentityProvider;

public class RedirectService(IOptions<HomeConfig> homeConfig)
{
    public IActionResult GetResultForPublicOnlyPage(ClaimsPrincipal user, Func<string, RedirectResult> redirect, Func<PageResult> page)
    {
        if (user.Identity is { IsAuthenticated: true })
        {
            return redirect(homeConfig.Value.Url);
        }

        return page();
    }
}