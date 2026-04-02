using Core.Entities;
using Core.Interfaces.IRepositories;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            return await context.Tasks.Include(t => t.Assignee).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(TaskQuery query)
        {
            var taskQuery = context.Tasks.Include(t => t.Assignee).AsQueryable();

            if (query.Status.HasValue)
                taskQuery = taskQuery.Where(s => s.Status == query.Status);

            if (query.Priority.HasValue)
                taskQuery = taskQuery.Where(s => s.Priority == query.Priority);

            if (query.AssigneeId.HasValue)
                taskQuery = taskQuery.Where(s => s.AssigneeId == query.AssigneeId);

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
                taskQuery = taskQuery.Where(t => t.Title.Contains(query.SearchTerm));

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