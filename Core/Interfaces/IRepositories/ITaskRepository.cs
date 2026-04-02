using Core.Entities;
using Core.Models;

namespace Core.Interfaces.IRepositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(TaskQuery query);
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task AddAsync(TaskItem task);
        void Update(TaskItem task);
        void Delete(TaskItem task);
        Task SaveChangesAsync();
    }
}