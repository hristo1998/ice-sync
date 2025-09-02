using IceSync.Core.Models;
using IceSync.Core.Models.Dtos;

namespace IceSync.Core.Services.Interfaces;

public interface IUniversalLoaderService
{
    // Workflows
    Task<List<Workflow>> GetWorkflowsAsync();
    Task<bool> RunWorkflowAsync(int workflowId, bool? waitOutput = null, bool? decodeOutputJsonString = null);


    // Workflow Executions
    Task<List<WorkflowExecution>> GetWorkflowExecutionsAsync(int? workflowId = null, DateTime? from = null, DateTime? to = null);
    Task<List<WorkflowExecutionStep>> GetExecutionStepsAsync(int executionId);
    Task<bool> RetryExecutionAsync(int executionId);


    // Workflow States
    Task<List<WorkflowState>> GetWorkflowStatesAsync();
    Task<WorkflowState?> GetWorkflowStateByIdAsync(int id);
    Task<WorkflowState?> CreateWorkflowStateAsync(WorkflowStateDto state);
    Task<WorkflowState?> UpdateWorkflowStateAsync(WorkflowStateDto state, string? externalUser = null);
    Task<bool> DeleteWorkflowStateAsync(int id, string? externalUser = null);
}