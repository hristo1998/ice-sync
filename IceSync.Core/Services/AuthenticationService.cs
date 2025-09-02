using IceSync.Core.Config;
using IceSync.Core.Models;
using IceSync.Core.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace IceSync.Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient httpClient;
    private readonly ApiSettings settings;

    private string? token;
    private DateTime expiry;

    private const string AuthV2Path = "v2/authenticate";
    private const string AuthenticationFailedErrorMessage = "Authentication failed: no access token acquired.";

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

        var response = await httpClient.PostAsJsonAsync($"{settings.BaseUrl}/{AuthV2Path}", credentials);
        response.EnsureSuccessStatusCode();

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(); 

        if (authResponse == null || string.IsNullOrEmpty(authResponse.AccessToken))
            throw new InvalidOperationException(AuthenticationFailedErrorMessage);

        token = authResponse.AccessToken;

        expiry = DateTime.UtcNow.AddSeconds(authResponse.ExpiresIn - 60); // subtract 1 min safety buffer

        return token;
    }
}
