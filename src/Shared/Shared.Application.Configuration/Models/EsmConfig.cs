using Utconnect.Common.Configurations.Models;

namespace Shared.Application.Configuration.Models;

public class EsmConfig : ISiteConfig
{
    public string Url { get; set; } = default!;
}