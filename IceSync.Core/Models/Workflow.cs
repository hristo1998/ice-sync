using IceSync.Core.Models.Dtos;

namespace IceSync.Core.Models;

public class Workflow
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

    public MultiExecBehavior? MultiExecBehavior { get; set; }

    public int? ExecutionRetriesCount { get; set; }

    public int? ExecutionRetriesPeriod { get; set; }

    public TimeUnit? ExecutionRetriesPeriodTimeUnit { get; set; }

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

    public static Workflow MapToDomain(WorkflowDto dto) => new Workflow
    {
        Id = dto.Id,
        Name = dto.Name,
        Version = dto.Version,
        VersionName = dto.VersionName,
        VersionNotes = dto.VersionNotes,
        Description = dto.Description,
        IsActive = dto.IsActive,
        CreationDateTime = dto.CreationDateTime,
        CreationUserId = dto.CreationUserId,
        OwnerUserId = dto.OwnerUserId,
        MultiExecBehavior = dto.MultiExecBehavior switch
        {
            nameof(Models.MultiExecBehavior.Parallel) => Models.MultiExecBehavior.Parallel, // Deducted from api responses
            nameof(Models.MultiExecBehavior.Sequential) => Models.MultiExecBehavior.Sequential, // Guessed. Keep filling the enum with reponses from the api
            _ => Models.MultiExecBehavior.Unknown
        },
        ExecutionRetriesCount = dto.ExecutionRetriesCount,
        ExecutionRetriesPeriod = dto.ExecutionRetriesPeriod,
        ExecutionRetriesPeriodTimeUnit = dto.ExecutionRetriesPeriodTimeUnit switch
        {
            nameof(TimeUnit.Second) => TimeUnit.Second,
            nameof(TimeUnit.Minute) => TimeUnit.Minute, // The only one confirmed as an actual Universal Loader api response. 
            nameof(TimeUnit.Hour) => TimeUnit.Hour,
            nameof(TimeUnit.Day) => TimeUnit.Day,
            nameof(TimeUnit.Week) => TimeUnit.Week,
            nameof(TimeUnit.Month) => TimeUnit.Month,
            nameof(TimeUnit.Year) => TimeUnit.Year,
            _ => TimeUnit.Other // All are guessed based on "Minute". Keep adding/changing as new responses are discovered from the api.
        },
        ProgressiveRetryMultiplier = dto.ProgressiveRetryMultiplier,
        MaxRetryInterval = dto.MaxRetryInterval,
        WorkflowGroupId = dto.WorkflowGroupId,
        CanStoreSuccessExecutionData = dto.CanStoreSuccessExecutionData,
        SuccessExecutionDataRetentionPeriodDays = dto.SuccessExecutionDataRetentionPeriodDays,
        CanStoreWarningExecutionData = dto.CanStoreWarningExecutionData,
        WarningExecutionDataRetentionPeriodDays = dto.WarningExecutionDataRetentionPeriodDays,
        CanStoreFailureExecutionData = dto.CanStoreFailureExecutionData,
        FailureExecutionDataRetentionPeriodDays = dto.FailureExecutionDataRetentionPeriodDays,
        EnableConnectorLogging = dto.EnableConnectorLogging
    };
}


