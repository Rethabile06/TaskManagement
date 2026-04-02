using Core.Enums;
using TaskStatus = Core.Enums.TaskStatus;

namespace Core.Entities
{
    public class TaskItem : BaseEntity
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }

        // Foreign Key
        public Guid? AssigneeId { get; set; } 

        // Navigation Property
        public TeamMember? Assignee { get; set; }
    }
}
