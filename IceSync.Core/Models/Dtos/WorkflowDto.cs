namespace IceSync.Core.Models.Dtos;

public class WorkflowDto
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Version { get; set; }

    public string? VersionName { get; set; }

    public string? VersionNotes { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDateTime { get; set; }

    public int CreationUserId { get; set; }

    public int OwnerUserId { get; set; }

    public string? MultiExecBehavior { get; set; }

    public int? ExecutionRetriesCount { get; set; }

    public int? ExecutionRetriesPeriod { get; set; }

    public string? ExecutionRetriesPeriodTimeUnit { get; set; }

    public double? ProgressiveRetryMultiplier { get; set; }

    public int? MaxRetryInterval { get; set; }

    public int WorkflowGroupId { get; set; }

    public bool CanStoreSuccessExecutionData { get; set; }

    public int? SuccessExecutionDataRetentionPeriodDays { get; set; }

    public bool CanStoreWarningExecutionData { get; set; }

    public int? WarningExecutionDataRetentionPeriodDays { get; set; }

    public bool CanStoreFailureExecutionData { get; set; }

    public int? FailureExecutionDataRetentionPeriodDays { get; set; }

    public bool EnableConnectorLogging { get; set; }
}
