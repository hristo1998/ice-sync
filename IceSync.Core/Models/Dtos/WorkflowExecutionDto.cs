namespace IceSync.Core.Models.Dtos;

public class WorkflowExecutionDto
{
    public int Id { get; set; }

    public int WorkflowId { get; set; }

    public string? WorkflowName { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public string? ExecutionStatus { get; set; }

    public string? WorkflowExecutionType { get; set; }

    public string? ExecutionUserName { get; set; }

    public int? RetryNumber { get; set; }
}
