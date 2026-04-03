using Core.Entities;
using Core.Enums;
using Core.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Core.Enums.TaskStatus;

namespace Infrastructure.Repositories
{
    public class TaskRepository(TaskDbContext context) : ITaskRepository
    {
        public async Task AddAsync(TaskItem task)
        {
            await context.Tasks.AddAsync(task);
        }

        public void Delete(TaskItem task)
        {
            context.Remove(task);
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await context.Tasks.Include(t => t.Member).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(TaskStatus? status, TaskPriority? priority, Guid? memberId, string? searchTerm)
        {
            var taskQuery = context.Tasks.Include(t => t.Member).AsQueryable();

            if (status.HasValue)
                taskQuery = taskQuery.Where(s => s.Status == status);

            if (priority.HasValue)
                taskQuery = taskQuery.Where(s => s.Priority == priority);

            if (memberId.HasValue)
                taskQuery = taskQuery.Where(s => s.MemberId == memberId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
                taskQuery = taskQuery.Where(t => t.Title.Contains(searchTerm));

            return await taskQuery.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Update(TaskItem task)
        {
            context.Entry(task).State = EntityState.Modified;
            context.Entry(task).Property(t => t.CreatedAt).IsModified = false;
        }
    }
}