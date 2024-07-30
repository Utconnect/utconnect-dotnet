using System.Text.Json.Serialization;

namespace Oidc.Domain.Models;

public class ExchangeTokenRequest
{
    [JsonPropertyName("scope")]
    public string Scope { get; set; } = null!;

    [JsonPropertyName("grant_type")]
    public string GrantType { get; set; } = null!;

    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = null!;

    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; } = null!;
}