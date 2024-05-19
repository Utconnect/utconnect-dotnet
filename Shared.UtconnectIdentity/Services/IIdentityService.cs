using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Shared.UtconnectIdentity.Models;

namespace Shared.UtconnectIdentity.Services;

public interface IIdentityService
{
    IIdentity? GetCurrent();

    public (string AuthenticationScheme, ClaimsPrincipal, AuthenticationProperties authProperties) GetNewClaims(
        IUser user);
}