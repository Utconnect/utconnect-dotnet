namespace Shared.Application.Configuration;

public class HomeConfig : ISiteConfig
{
    public string Url { get; set; } = default!;
}