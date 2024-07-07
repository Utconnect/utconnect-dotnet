using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityProvider.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Authentication.Extensions;
using Shared.Authentication.Models;
using Shared.Services.Abstractions;

namespace Shared.Authentication.Services;

public class JwtService(IConfiguration configuration, IDateTime dateTime) : IJwtService
{
    public GeneratedToken CreateToken(User user)
    {
        DateTime now = dateTime.Now;
        DateTime expiration = now.AddMinutes(double.Parse(configuration["ExpirationMinutes"] ?? "0"));
        IEnumerable<Claim> claims = user.CreateClaims(now);
        SigningCredentials credentials = CreateSigningCredentials();

        JwtSecurityToken token = CreateJwtToken(claims, credentials, expiration);
        JwtSecurityTokenHandler tokenHandler = new();

        return new GeneratedToken
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = expiration
        };
    }

    private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims,
        SigningCredentials credentials,
        DateTime expiration) =>
        new(configuration["Issuer"], configuration["Audience"], claims, expires: expiration,
            signingCredentials: credentials);

    private SigningCredentials CreateSigningCredentials() => new(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Key"] ?? string.Empty)),
        SecurityAlgorithms.HmacSha256);
}