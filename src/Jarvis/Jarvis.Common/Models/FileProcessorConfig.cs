using Utconnect.Common.Configurations.Models;

namespace Jarvis.Common.Models;

public class FileProcessorConfig : ISiteConfig
{
    public string Url { get; set; } = default!;
}