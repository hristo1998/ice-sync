using IceSync.Api.ViewModels;
using IceSync.Core.Models;
using IceSync.Core.Models.Dtos;
using IceSync.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class MainController : ControllerBase
{
    private readonly IUniversalLoaderService ulService;

    public MainController(IUniversalLoaderService ulService)
    {
        this.ulService = ulService;
    }

    // Workflows
    [HttpGet("workflows")]
    public async Task<IActionResult> GetWorkflows()
    {
        var modelList = await ulService.GetWorkflowsAsync();
        return Ok(modelList.Select(WorkflowVm.MapToViewModel));
    }


    [HttpPost("workflows/{workflowId}/run")]
    public async Task<IActionResult> RunWorkflow(
        int workflowId, 
        [FromQuery] bool? waitOutput, 
        [FromQuery] bool? decodeOutputJsonString) =>
        Ok(await ulService.RunWorkflowAsync(workflowId, waitOutput, decodeOutputJsonString));

    // Executions
    [HttpGet("workflows/executions")]
    public async Task<IActionResult> GetExecutions(
        [FromQuery] int? workflowId, 
        [FromQuery] DateTime? 
        execStartFromUtcDateTime, 
        [FromQuery] DateTime? execStartToUtcDateTime) =>
        Ok(await ulService.GetWorkflowExecutionsAsync(workflowId, execStartFromUtcDateTime, execStartToUtcDateTime));

    [HttpGet("workflows/executions/{executionId}/steps")]
    public async Task<IActionResult> GetExecutionSteps(int executionId) =>
        Ok(await ulService.GetExecutionStepsAsync(executionId));

    [HttpPost("workflows/executions/{executionId}/retry")]
    public async Task<IActionResult> RetryExecution(int executionId) =>
        Ok(await ulService.RetryExecutionAsync(executionId));

    // Workflow States
    [HttpGet("workflow-states")]
    public async Task<IActionResult> GetStates() =>
        Ok(await ulService.GetWorkflowStatesAsync());

    [HttpGet("workflow-states/{id}")]
    public async Task<IActionResult> GetStateById(int id) =>
        Ok(await ulService.GetWorkflowStateByIdAsync(id));

    [HttpPost("workflow-states")]
    public async Task<IActionResult> CreateState([FromBody] WorkflowStateDto state) =>
        Ok(await ulService.CreateWorkflowStateAsync(state));

    [HttpPut("workflow-states")]
    public async Task<IActionResult> UpdateState([FromBody] WorkflowStateDto state, [FromQuery] string? externalUser) =>
        Ok(await ulService.UpdateWorkflowStateAsync(state, externalUser));

    [HttpDelete("workflow-states/{id}")]
    public async Task<IActionResult> DeleteState(int id, [FromQuery] string? externalUser) =>
        Ok(await ulService.DeleteWorkflowStateAsync(id, externalUser));
}