using IceSync.Core.Config;
using IceSync.Core.Models;
using IceSync.Core.Models.Dtos;
using IceSync.Core.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace IceSync.Core.Services;

public class UniversalLoaderService : IUniversalLoaderService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationService _authService;
    private readonly ApiSettings _settings;

    public UniversalLoaderService(HttpClient httpClient, IAuthenticationService authService, IOptions<ApiSettings> options)
    {
        _httpClient = httpClient;
        _authService = authService;
        _settings = options.Value;
    }

    /// <summary>
    /// Ensures the HttpClient has a valid Bearer token.
    /// </summary>
    private async Task PrepareClientAsync()
    {
        var token = await _authService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    // -----------------------------------------------------------
    // Workflows
    // -----------------------------------------------------------
    public async Task<List<Workflow>> GetWorkflowsAsync()
    {
        await PrepareClientAsync();
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/workflows");
        response.EnsureSuccessStatusCode();
        var apiResponse = await response.Content.ReadFromJsonAsync<List<WorkflowDto>>() ?? new List<WorkflowDto>();

        var resonse = apiResponse.Select(dto => Workflow.MapToDomain(dto)).ToList();

        return resonse;
    }

    public async Task<bool> RunWorkflowAsync(int workflowId, bool? waitOutput = null, bool? decodeOutputJsonString = null)
    {
        await PrepareClientAsync();

        var url = $"{_settings.BaseUrl}/workflows/{workflowId}/run";
        var query = new List<string>();
        if (waitOutput.HasValue) query.Add($"waitOutput={waitOutput.Value}");
        if (decodeOutputJsonString.HasValue) query.Add($"decodeOutputJsonString={decodeOutputJsonString.Value}");
        if (query.Any()) url += "?" + string.Join("&", query);

        var response = await _httpClient.PostAsync(url, null);
        return response.IsSuccessStatusCode;
    }

    // -----------------------------------------------------------
    // Workflow Executions
    // -----------------------------------------------------------
    public async Task<List<WorkflowExecution>> GetWorkflowExecutionsAsync(int? workflowId = null, DateTime? from = null, DateTime? to = null)
    {
        await PrepareClientAsync();

        var query = new List<string>();
        if (workflowId.HasValue) query.Add($"workflowId={workflowId.Value}");
        if (from.HasValue) query.Add($"execStartFromUtcDateTime={from.Value:o}");
        if (to.HasValue) query.Add($"execStartToUtcDateTime={to.Value:o}");

        var url = $"{_settings.BaseUrl}/workflows/executions";
        if (query.Any()) url += "?" + string.Join("&", query);

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<WorkflowExecution>>() ?? new List<WorkflowExecution>();
    }

    public async Task<List<object>> GetExecutionStepsAsync(int executionId)
    {
        await PrepareClientAsync();
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/workflows/executions/{executionId}/steps");
        response.EnsureSuccessStatusCode();

        // swagger doesn’t define schema for steps, so keep as object[]
        return await response.Content.ReadFromJsonAsync<List<object>>() ?? new List<object>();
    }

    public async Task<bool> RetryExecutionAsync(int executionId)
    {
        await PrepareClientAsync();
        var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/workflows/executions/{executionId}/retry", null);
        return response.IsSuccessStatusCode;
    }

    // -----------------------------------------------------------
    // Workflow States
    // -----------------------------------------------------------
    public async Task<List<WorkflowState>> GetWorkflowStatesAsync()
    {
        await PrepareClientAsync();
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/workflow-states");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<WorkflowState>>() ?? new List<WorkflowState>();
    }

    public async Task<WorkflowState?> GetWorkflowStateByIdAsync(int id)
    {
        await PrepareClientAsync();
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/workflow-states/{id}");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<WorkflowState>();
    }

    public async Task<WorkflowState?> CreateWorkflowStateAsync(WorkflowState state)
    {
        await PrepareClientAsync();
        var response = await _httpClient.PostAsJsonAsync($"{_settings.BaseUrl}/workflow-states", state);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<WorkflowState>();
    }

    public async Task<WorkflowState?> UpdateWorkflowStateAsync(WorkflowState state, string? externalUser = null)
    {
        await PrepareClientAsync();
        var url = $"{_settings.BaseUrl}/workflow-states";
        if (!string.IsNullOrEmpty(externalUser))
            url += $"?externalUser={externalUser}";

        var response = await _httpClient.PutAsJsonAsync(url, state);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<WorkflowState>();
    }

    public async Task<bool> DeleteWorkflowStateAsync(int id, string? externalUser = null)
    {
        await PrepareClientAsync();
        var url = $"{_settings.BaseUrl}/workflow-states/{id}";
        if (!string.IsNullOrEmpty(externalUser))
            url += $"?externalUser={externalUser}";

        var response = await _httpClient.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }
}
