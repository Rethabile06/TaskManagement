using Core.Enums;
using TaskStatus = Core.Enums.TaskStatus;

namespace Core.Models.Task
{
    public class TaskResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid? MemberId { get; set; }
        public string? MemberName { get; set; }
    }
}