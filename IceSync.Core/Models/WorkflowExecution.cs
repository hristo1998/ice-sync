using IceSync.Core.Models.Dtos;
using System;

namespace IceSync.Core.Models;

public class WorkflowExecution
{
    public int Id { get; set; }

    public int WorkflowId { get; set; }

    public string? WorkflowName { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public ExecutionStatus ExecutionStatus { get; set; }

    public WorkflowExecutionType WorkflowExecutionType { get; set; }

    public string? ExecutionUserName { get; set; }

    public int? RetryNumber { get; set; }

    public static WorkflowExecution MapToDomain(WorkflowExecutionDto dto) => new WorkflowExecution
    {
        Id = dto.Id,
        WorkflowId = dto.WorkflowId,
        WorkflowName = dto.WorkflowName,
        StartDateTime = dto.StartDateTime,
        StopDateTime = dto.StopDateTime,
        ExecutionStatus = dto.ExecutionStatus switch
        {
            "Success" => ExecutionStatus.Success, // The only one retrieved from the Universal Loader api
            _ => ExecutionStatus.Unknown // Default option. Keep adding options as you discover them.
        },
        WorkflowExecutionType = dto.WorkflowExecutionType switch
        {
            "Api" => WorkflowExecutionType.Api, // The only one retrieved from the Universal Loader api
            _ => WorkflowExecutionType.Unknown // Default option. Keep adding options as you discover them.
        },
        ExecutionUserName = dto.ExecutionUserName,
        RetryNumber = dto.RetryNumber,
    };
}
