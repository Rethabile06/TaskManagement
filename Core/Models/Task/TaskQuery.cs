using Core.Enums;
using TaskStatus = Core.Enums.TaskStatus;

namespace Core.Models.Task
{
    public class TaskQuery
    {
        public string? SearchTerm { get; set; }
        public TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public Guid? MemberId { get; set; }
    }
}