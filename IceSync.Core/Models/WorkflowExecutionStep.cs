using IceSync.Core.Models.Dtos;

namespace IceSync.Core.Models;

public class WorkflowExecutionStep
{
    public int Id { get; set; }

    public int WorkflowExecutionId { get; set; }

    public int WorkflowStepId { get; set; }

    public string StepName { get; set; } = string.Empty;

    public string StepType { get; set; } = string.Empty;

    public DateTime StartDateTime { get; set; }

    public DateTime StopDateTime { get; set; }

    public string ExecutionStatus { get; set; } = string.Empty;

    public List<string> Messages { get; set; } = new();

    public static WorkflowExecutionStep ToDomain(WorkflowExecutionStepDto dto)
    {
        return new WorkflowExecutionStep
        {
            Id = dto.Id,
            WorkflowExecutionId = dto.WorkflowExecutionId,
            WorkflowStepId = dto.WorkflowStepId,
            StepName = dto.StepName,
            StepType = dto.StepType,
            StartDateTime = dto.StartDateTime,
            StopDateTime = dto.StopDateTime,
            ExecutionStatus = dto.ExecutionStatus,
            Messages = dto.Messages.ToList()
        };
    }
}
