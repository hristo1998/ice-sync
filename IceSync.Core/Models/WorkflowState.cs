using IceSync.Core.Models.Dtos;

namespace IceSync.Core.Models;

public class WorkflowState
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsLocked { get; set; }

    public object? State { get; set; }  // swagger had JToken, keep flexible

    public DateTime CreationDateTime { get; set; }

    public int CreationUserId { get; set; }

    public bool IsConfigTable { get; set; }

    public string? Description { get; set; }

    public bool IsSystemState { get; set; }

    public static WorkflowState MapToDomain(WorkflowStateDto dto) => new WorkflowState
    {
        Id = dto.Id,
        Name = dto.Name,
        IsLocked = dto.IsLocked,
        State = dto.State,
        CreationDateTime = dto.CreationDateTime,
        CreationUserId = dto.CreationUserId,
        IsConfigTable = dto.IsConfigTable,
        Description = dto.Description,
        IsSystemState = dto.IsSystemState
    };
}
