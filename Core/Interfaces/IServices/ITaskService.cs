using Core.Common;
using Core.Models.Task;

namespace Core.Interfaces.IServices
{
    public interface ITaskService
    {
        Task<Result<IEnumerable<TaskResponse>>> GetAllTasksAsync(TaskQuery query);
        Task<Result<TaskResponse>> GetTaskByIdAsync(Guid id);
        Task<Result<TaskResponse>> CreateTaskAsync(CreateTaskRequest request);
        Task<Result> UpdateTaskAsync(Guid id, UpdateTaskRequest request);
        Task<Result> DeleteTaskAsync(Guid id);
        Task<Result> AssignTaskAsync(Guid taskId, Guid memberId);
    }
}
