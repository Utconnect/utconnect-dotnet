using Newtonsoft.Json;
using Shared.Authentication.Exceptions;
using Shared.Authentication.Models;

namespace Shared.Authentication.Services;

public static class CofferService
{
    public static async Task<string> GetKey(string? cofferUrl, string app, string secretName)
    {
        if (string.IsNullOrEmpty(cofferUrl))
        {
            throw new UnableRetrieveJwtException("Coffer URL is empty");
        }

        var requestUrl = $"{cofferUrl}/secret/be/{app}/{secretName}";
        HttpClient client = new();
        HttpResponseMessage response = await client.GetAsync(requestUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new UnableRetrieveJwtException("Response status is not success");
        }

        try
        {
            string content = await response.Content.ReadAsStringAsync();
            CofferResponse? jwtKey = JsonConvert.DeserializeObject<CofferResponse>(content);
            if (jwtKey == null)
            {
                throw new UnableRetrieveJwtException("Retrieved data is null");
            }

            return jwtKey.Data;
        }
        catch (Exception)
        {
            throw new UnableRetrieveJwtException("Cannot decode response");
        }
    }
}