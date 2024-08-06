using Utconnect.Common.Configurations.Models;

namespace Shared.Application.Configuration.Models;

public class HomeConfig : ISiteConfig
{
    public string Url { get; set; } = default!;
    public string TokenPath { get; set; } = default!;
    public string LogoutPath { get; set; } = default!;
}