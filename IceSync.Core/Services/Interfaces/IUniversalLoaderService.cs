using IceSync.Core.Models;

namespace IceSync.Core.Services.Interfaces;

public interface IUniversalLoaderService
{
    // Workflows
    Task<List<Workflow>> GetWorkflowsAsync();
    Task<bool> RunWorkflowAsync(int workflowId, bool? waitOutput = null, bool? decodeOutputJsonString = null);


    // Workflow Executions
    Task<List<WorkflowExecution>> GetWorkflowExecutionsAsync(int? workflowId = null, DateTime? from = null, DateTime? to = null);
    Task<List<object>> GetExecutionStepsAsync(int executionId);
    Task<bool> RetryExecutionAsync(int executionId);


    // Workflow States
    Task<List<WorkflowState>> GetWorkflowStatesAsync();
    Task<WorkflowState?> GetWorkflowStateByIdAsync(int id);
    Task<WorkflowState?> CreateWorkflowStateAsync(WorkflowState state);
    Task<WorkflowState?> UpdateWorkflowStateAsync(WorkflowState state, string? externalUser = null);
    Task<bool> DeleteWorkflowStateAsync(int id, string? externalUser = null);
}