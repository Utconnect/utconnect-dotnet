using Shared.Application.Configuration.Models;

namespace Jarvis.Common.Models;

public class JarvisConfig : ISiteConfig
{
    public string Url { get; set; } = default!;
}