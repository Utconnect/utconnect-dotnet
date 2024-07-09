using System.Text.Json.Serialization;

namespace Oidc.Domain.Models;

public class UserInfoResponse
{
    [JsonPropertyName("sub")]
    public string Sub { get; set; } = null!;

    [JsonPropertyName("iss")]
    public string Iss { get; set; } = null!;

    [JsonPropertyName("username")]
    public string UserName { get; set; } = null!;
}