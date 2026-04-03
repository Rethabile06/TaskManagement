using Core.Entities;
using Core.Enums;
using TaskStatus = Core.Enums.TaskStatus;

namespace Core.Interfaces.IRepositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(TaskStatus? status, TaskPriority? priority, Guid? memberId, string? searchTerm);
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task AddAsync(TaskItem task);
        void Update(TaskItem task);
        void Delete(TaskItem task);
        Task SaveChangesAsync();
    }
}