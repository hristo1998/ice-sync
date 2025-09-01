using IceSync.Core.Config;
using IceSync.Core.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace IceSync.Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient httpClient;
    private readonly ApiSettings settings;

    private string? token;
    private DateTime expiry;

    public AuthenticationService(HttpClient httpClient, IOptions<ApiSettings> options)
    {
        this.httpClient = httpClient;
        settings = options.Value;
    }

    public async Task<string> GetTokenAsync()
    {
        if (token != null && expiry > DateTime.UtcNow)
            return token;

        var credentials = new
        {
            apiCompanyId = settings.ApiCompanyId,
            apiUserId = settings.ApiUserId,
            apiUserSecret = settings.ApiUserSecret
        };

        var response = await httpClient.PostAsJsonAsync($"{settings.BaseUrl}/v2/authenticate", credentials);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        token = json.GetProperty("access_token").GetString();
        var expiresIn = json.GetProperty("expires_in").GetInt32();
        expiry = DateTime.UtcNow.AddSeconds(expiresIn - 60); // subtract 1 min safety buffer

        return token!;
    }
}
