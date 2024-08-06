using Shared.Authentication.Configurations;
using Utconnect.Common.Configurations.Models;

namespace Shared.Application.Configuration.Models;

public class OidcConfig : ISiteConfig
{
    public JwtConfig Jwt { get; set; } = null!;
    public string Url { get; set; } = default!;
    public string TokenPath { get; set; } = default!;
    public string UserInfoPath { get; set; } = default!;
}