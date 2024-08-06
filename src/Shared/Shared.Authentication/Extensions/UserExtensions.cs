using System.Security.Claims;
using IdentityProvider.Domain.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Utconnect.Common.Extensions;

namespace Shared.Authentication.Extensions;

public static class UserExtensions
{
    public static Claim[] CreateClaims(this User user, DateTime now) =>
    [
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Name, user.Name),
        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
    ];
}