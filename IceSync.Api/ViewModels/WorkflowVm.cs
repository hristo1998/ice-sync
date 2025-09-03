using IceSync.Core.Models;

namespace IceSync.Api.ViewModels
{
    public class WorkflowVm
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsActive { get; set; }


        public string? MultiExecBehavior { get; set; }


        public static WorkflowVm MapToViewModel(Workflow model)
        {
            return new WorkflowVm 
            { 
                Id = model.Id, 
                Name = model.Name, 
                IsActive = model.IsActive, 
                MultiExecBehavior = model.MultiExecBehavior.ToString(), 
            };
        }
    }
}
