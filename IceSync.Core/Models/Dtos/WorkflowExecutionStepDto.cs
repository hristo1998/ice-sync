namespace IceSync.Core.Models.Dtos;

public class WorkflowExecutionStepDto
{
    public int Id { get; set; }

    public int WorkflowExecutionId { get; set; }

    public int WorkflowStepId { get; set; }

    public string StepName { get; set; } = string.Empty;

    public string StepType { get; set; } = string.Empty; // e.g. "StartingStep". Maybe enum?

    public DateTime StartDateTime { get; set; }

    public DateTime StopDateTime { get; set; }

    public string ExecutionStatus { get; set; } = string.Empty; // e.g. "Success". Maybe enum?

    public List<string> Messages { get; set; } = new();
}
