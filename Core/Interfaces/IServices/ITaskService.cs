using Core.Common;
using Core.Enums;
using Core.Models;
using TaskStatus = Core.Enums.TaskStatus;

namespace Core.Interfaces.IServices
{
    public interface ITaskService
    {
        Task<Result<IEnumerable<TaskDto>>> GetAllTasksAsync(TaskQuery query);
        Task<Result<TaskDto>> GetTaskByIdAsync(Guid id);
        Task<Result<TaskDto>> CreateTaskAsync(TaskDto dto);
        Task<Result> UpdateTaskAsync(Guid id, TaskDto dto);
        Task<Result> DeleteTaskAsync(Guid id);
        Task<Result> AssignTaskAsync(Guid taskId, Guid assigneeId);
        Task<Result> UpdateTaskPriorityAsync(Guid taskId, TaskPriority priority);
        Task<Result> UpdateTaskStatusAsync(Guid taskId, TaskStatus status);
    }
}
