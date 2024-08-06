using Utconnect.Common.Configurations.Models;

namespace Jarvis.Common.Models;

public class JarvisConfig : ISiteConfig
{
    public string Url { get; set; } = default!;
}