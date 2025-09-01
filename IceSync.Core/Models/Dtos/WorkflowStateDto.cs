namespace IceSync.Core.Models.Dtos;

public class WorkflowStateDto
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
}
