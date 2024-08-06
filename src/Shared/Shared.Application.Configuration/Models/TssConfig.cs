using Utconnect.Common.Configurations.Models;

namespace Shared.Application.Configuration.Models;

public class TssConfig : ISiteConfig
{
    public string Url { get; set; } = default!;
}