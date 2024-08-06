using Utconnect.Common.Configurations.Models;

namespace Pince.Models;

public class FileManagerConfig : ISiteConfig
{
    public string Url { get; set; } = default!;
}