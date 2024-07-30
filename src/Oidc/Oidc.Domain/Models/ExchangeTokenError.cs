using System.Text.Json.Serialization;

namespace Oidc.Domain.Models;

public class ExchangeTokenError
{
    [JsonPropertyName("error")]
    public string Error { get; set; } = null!;

    [JsonPropertyName("error_description")]
    public string ErrorDescription { get; set; } = null!;

    [JsonPropertyName("error_uri")]
    public int ErrorUri { get; set; }
}