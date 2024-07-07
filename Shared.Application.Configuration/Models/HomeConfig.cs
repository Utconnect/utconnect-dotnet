namespace Shared.Application.Configuration.Models;

public class HomeConfig : ISiteConfig
{
    public string Url { get; set; } = default!;
    public string ReturnUrl { get; set; } = default!;
}