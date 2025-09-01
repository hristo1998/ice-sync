using IceSync.Core.Models;
using IceSync.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MainController : ControllerBase
{
    private readonly IUniversalLoaderService _ulService;

    public MainController(IUniversalLoaderService ulService)
    {
        _ulService = ulService;
    }

    // Workflows
    [HttpGet("workflows")]
    public async Task<IActionResult> GetWorkflows() =>
        Ok(await _ulService.GetWorkflowsAsync());

    [HttpPost("workflows/{workflowId}/run")]
    public async Task<IActionResult> RunWorkflow(int workflowId, [FromQuery] bool? waitOutput, [FromQuery] bool? decodeOutputJsonString) =>
        Ok(await _ulService.RunWorkflowAsync(workflowId, waitOutput, decodeOutputJsonString));

    // Executions
    [HttpGet("workflows/executions")]
    public async Task<IActionResult> GetExecutions([FromQuery] int? workflowId, [FromQuery] DateTime? execStartFromUtcDateTime, [FromQuery] DateTime? execStartToUtcDateTime) =>
        Ok(await _ulService.GetWorkflowExecutionsAsync(workflowId, execStartFromUtcDateTime, execStartToUtcDateTime));

    [HttpGet("workflows/executions/{executionId}/steps")]
    public async Task<IActionResult> GetExecutionSteps(int executionId) =>
        Ok(await _ulService.GetExecutionStepsAsync(executionId));

    [HttpPost("workflows/executions/{executionId}/retry")]
    public async Task<IActionResult> RetryExecution(int executionId) =>
        Ok(await _ulService.RetryExecutionAsync(executionId));

    // Workflow States
    [HttpGet("workflow-states")]
    public async Task<IActionResult> GetStates() =>
        Ok(await _ulService.GetWorkflowStatesAsync());

    [HttpGet("workflow-states/{id}")]
    public async Task<IActionResult> GetStateById(int id) =>
        Ok(await _ulService.GetWorkflowStateByIdAsync(id));

    [HttpPost("workflow-states")]
    public async Task<IActionResult> CreateState([FromBody] WorkflowState state) =>
        Ok(await _ulService.CreateWorkflowStateAsync(state));

    [HttpPut("workflow-states")]
    public async Task<IActionResult> UpdateState([FromBody] WorkflowState state, [FromQuery] string? externalUser) =>
        Ok(await _ulService.UpdateWorkflowStateAsync(state, externalUser));

    [HttpDelete("workflow-states/{id}")]
    public async Task<IActionResult> DeleteState(int id, [FromQuery] string? externalUser) =>
        Ok(await _ulService.DeleteWorkflowStateAsync(id, externalUser));
}