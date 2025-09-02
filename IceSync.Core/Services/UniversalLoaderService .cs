using IceSync.Core.Config;
using IceSync.Core.Models;
using IceSync.Core.Models.Dtos;
using IceSync.Core.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace IceSync.Core.Services;

public class UniversalLoaderService : IUniversalLoaderService
{
    private readonly HttpClient httpClient;
    private readonly IAuthenticationService authService;
    private readonly ApiSettings settings;

    private const string BearerScheme = "Bearer";

    // Workflows
    private const string WorkflowsEndpoint = "/workflows";
    private static readonly string RunWorkflowEndpoint = $"/{WorkflowsEndpoint}/{{0}}/run";

    // Executions
    private const string WorkflowExecutionsEndpoint = "/workflows/executions";
    private static readonly string ExecutionStepsEndpoint = $"/{WorkflowExecutionsEndpoint}/{{0}}/steps";
    private static readonly string RetryExecutionEndpoint = $"/{WorkflowExecutionsEndpoint}/{{0}}/retry";

    // Workflow States
    private const string WorkflowStatesEndpoint = "/workflow-states";

    public UniversalLoaderService(HttpClient httpClient, IAuthenticationService authService, IOptions<ApiSettings> options)
    {
        this.httpClient = httpClient;
        this.authService = authService;
        settings = options.Value;
    }

    // Workflows
    public async Task<List<Workflow>> GetWorkflowsAsync()
    {
        var request = await CreateRequestAsync(HttpMethod.Get, WorkflowsEndpoint);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var modelDto = await response.Content.ReadFromJsonAsync<List<WorkflowDto>>() ?? new List<WorkflowDto>();
        var result = modelDto.Select(Workflow.MapToDomain).ToList();
        return result;
    }

    public async Task<bool> RunWorkflowAsync(int workflowId, bool? waitOutput = null, bool? decodeOutputJsonString = null)
    {
        var url = string.Format(RunWorkflowEndpoint, workflowId);

        if (waitOutput.HasValue || decodeOutputJsonString.HasValue)
        {
            var parameters = new Dictionary<string, string?>();
            if (waitOutput.HasValue) parameters[nameof(waitOutput)] = waitOutput.Value.ToString();
            if (decodeOutputJsonString.HasValue) parameters[nameof(decodeOutputJsonString)] = decodeOutputJsonString.Value.ToString();
            url = QueryHelpers.AddQueryString(url, parameters);
        }

        var request = await CreateRequestAsync(HttpMethod.Post, url);
        var response = await httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    // Executions
    public async Task<List<WorkflowExecution>> GetWorkflowExecutionsAsync(
        int? workflowId = null, 
        DateTime? execStartFromUtcDateTime = null, 
        DateTime? execStartToUtcDateTime = null)
    {
        var url = WorkflowExecutionsEndpoint;

        if (workflowId.HasValue || execStartFromUtcDateTime.HasValue || execStartToUtcDateTime.HasValue)
        {
            var parameters = new Dictionary<string, string?>();
            if (workflowId.HasValue) parameters[nameof(workflowId)] = workflowId.Value.ToString();
            if (execStartFromUtcDateTime.HasValue) parameters[nameof(execStartFromUtcDateTime)] = execStartFromUtcDateTime.Value.ToString("O");
            if (execStartToUtcDateTime.HasValue) parameters[nameof(execStartToUtcDateTime)] = execStartToUtcDateTime.Value.ToString("O");

            url = QueryHelpers.AddQueryString(url, parameters);
        }

        var request = await CreateRequestAsync(HttpMethod.Get, url);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var modelDto = await response.Content.ReadFromJsonAsync<List<WorkflowExecutionDto>>() ?? new List<WorkflowExecutionDto>();
        var result = modelDto.Select(WorkflowExecution.MapToDomain).ToList();
        return result;
    }

    public async Task<List<WorkflowExecutionStep>> GetExecutionStepsAsync(int executionId)
    {
        var url = string.Format(ExecutionStepsEndpoint, executionId);
        var request = await CreateRequestAsync(HttpMethod.Get, url);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // Return raw element so you can decide later how to shape it
        var modelDto = await response.Content.ReadFromJsonAsync<List<WorkflowExecutionStepDto>>() ?? new List<WorkflowExecutionStepDto>();
        return modelDto.Select(WorkflowExecutionStep.ToDomain).ToList();
    }

    public async Task<bool> RetryExecutionAsync(int executionId)
    {
        var url = string.Format(RetryExecutionEndpoint, executionId);
        var request = await CreateRequestAsync(HttpMethod.Post, url);
        var response = await httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    // Workflow States
    public async Task<List<WorkflowState>> GetWorkflowStatesAsync()
    {
        var request = await CreateRequestAsync(HttpMethod.Get, WorkflowStatesEndpoint);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var modelDto = await response.Content.ReadFromJsonAsync<List<WorkflowStateDto>>() ?? new List<WorkflowStateDto>();
        return modelDto.Select(WorkflowState.MapToDomain).ToList();
    }

    public async Task<WorkflowState?> GetWorkflowStateByIdAsync(int id)
    {
        var url = $"{WorkflowStatesEndpoint}/{id}";
        var request = await CreateRequestAsync(HttpMethod.Get, url);
        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) return null;

        var modelDto = await response.Content.ReadFromJsonAsync<WorkflowStateDto>();

        return WorkflowState.MapToDomain(modelDto!);
    }

    public async Task<WorkflowState?> CreateWorkflowStateAsync(WorkflowStateDto state)
    {
        var request = await CreateRequestAsync(HttpMethod.Post, WorkflowStatesEndpoint, state);
        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) return null;

        var modelDto = await response.Content.ReadFromJsonAsync<WorkflowStateDto>();

        return WorkflowState.MapToDomain(modelDto!);
    }

    public async Task<WorkflowState?> UpdateWorkflowStateAsync(WorkflowStateDto state, string? externalUser = null)
    {
        var url = WorkflowStatesEndpoint;

        if (!string.IsNullOrWhiteSpace(externalUser))
        {
            var parameters = new Dictionary<string, string?> { [nameof(externalUser)] = externalUser };
            url = QueryHelpers.AddQueryString(url, parameters);
        }

        var request = await CreateRequestAsync(HttpMethod.Put, url, state);
        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) return null;

        var modelDto = await response.Content.ReadFromJsonAsync<WorkflowStateDto>();

        return WorkflowState.MapToDomain(modelDto!);
    }

    public async Task<bool> DeleteWorkflowStateAsync(int id, string? externalUser = null)
    {
        var url = $"{WorkflowStatesEndpoint}/{id}";

        if (!string.IsNullOrWhiteSpace(externalUser))
        {
            var parameters = new Dictionary<string, string?> { [nameof(externalUser)] = externalUser };
            url = QueryHelpers.AddQueryString(url, parameters);
        }

        var request = await CreateRequestAsync(HttpMethod.Delete, url);
        var response = await httpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    private async Task<HttpRequestMessage> CreateRequestAsync(
        HttpMethod method,
        string endpoint,
        object? content = null)
    {
        var token = await authService.GetTokenAsync();

        var request = new HttpRequestMessage(method, settings.BaseUrl + endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue(BearerScheme, token);

        if (content is not null)
        {
            request.Content = JsonContent.Create(content);
        }

        return request;
    }
}
