using Core.Enums;
using System.ComponentModel.DataAnnotations;
using TaskStatus = Core.Enums.TaskStatus;

namespace Core.Models.Task
{
    public class CreateTaskRequest
    {
        [Required(ErrorMessage = "Title is required")]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public Guid? MemberId { get; set; }
    }
}