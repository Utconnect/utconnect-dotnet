using Utconnect.Common.Configurations.Models;

namespace Shared.Application.Configuration.Models;

public class IdentityConfig : ISiteConfig
{
    public string Url { get; set; } = default!;
    public string LoginPath { get; set; } = default!;
    public string ManagePath { get; set; } = default!;
    public string LogoutPath { get; set; } = default!;
}