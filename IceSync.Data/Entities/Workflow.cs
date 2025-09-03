
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IceSync.Data.Entities;

public class Workflow
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int WorkflowId { get; set; }   // Unique identifier from Universal Loader

    [Required]
    public string WorkflowName { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public string MultiExecBehavior { get; set; } = string.Empty;
}