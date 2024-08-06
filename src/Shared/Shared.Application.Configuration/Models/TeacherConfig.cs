using Utconnect.Common.Configurations.Models;

namespace Shared.Application.Configuration.Models;

public class TeacherConfig : ISiteConfig
{
    public string Url { get; set; } = default!;
}