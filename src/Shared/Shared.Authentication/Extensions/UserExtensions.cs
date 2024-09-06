using System.Security.Claims;
using IdentityProvider.Domain.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Utconnect.Common.Extensions;

namespace Shared.Authentication.Extensions;

public static class UserExtensions
{
    public static Claim[] CreateClaims(this User user, DateTime now) =>
    [
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString()),
        new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new(JwtRegisteredClaimNames.Name, user.Name),
        new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
    ];
}