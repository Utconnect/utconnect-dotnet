namespace Shared.Swashbuckle;

public class SwashbuckleInfo(string title, string version)
{
    public string Title { get; set; } = title;
    public string Version { get; set; } = version;
}