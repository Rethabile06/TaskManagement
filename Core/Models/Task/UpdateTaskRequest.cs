using Core.Enums;
using TaskStatus = Core.Enums.TaskStatus;

namespace Core.Models.Task
{
    public class UpdateTaskRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public Guid? MemberId { get; set; }
    }
}