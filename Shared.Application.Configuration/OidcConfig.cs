using Shared.Authentication.Configurations;

namespace Shared.Application.Configuration;

public class OidcConfig : ISiteConfig
{
    public string Url { get; set; } = default!;
    public JwtConfig Jwt { get; set; } = null!;
}