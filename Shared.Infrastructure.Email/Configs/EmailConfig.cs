using Shared.Application.Configuration.Models;

namespace Shared.Infrastructure.Email.Configs;

public class EmailConfig : ISiteConfig
{
    public string Url { get; set; } = default!;

    public string SendPath { get; set; } = default!;
}