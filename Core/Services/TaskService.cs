using AutoMapper;
using Core.Common;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices;
using Core.Models;
using Microsoft.Extensions.Logging;
using TaskStatus = Core.Enums.TaskStatus;

namespace Core.Services
{
    public class TaskService(ITaskRepository taskRepository, ITeamMemberRepository memberRepository, IMapper mapper, ILogger<TaskService> logger) : ITaskService
    {
        public async Task<Result> AssignTaskAsync(Guid taskId, Guid assigneeId)
        {
            logger.LogInformation("Assigning task {TaskId} to member {MemberId}", taskId, assigneeId);

            var task = await taskRepository.GetByIdAsync(taskId);
            if (task is null)
            {
                logger.LogWarning("Task {TaskId} not found", taskId);
                return Result.Failure("Task not found");
            }

            var member = await memberRepository.GetByIdAsync(assigneeId);
            if (member is null)
            {
                logger.LogWarning("Team member {MemberId} not found", assigneeId);
                return Result.Failure("Team member not found");
            }

            task.AssigneeId = member.Id;
            task.UpdatedAt = DateTime.UtcNow;

            taskRepository.Update(task);
            await taskRepository.SaveChangesAsync();

            logger.LogInformation("Task {TaskId} assigned to member {MemberId}", taskId, assigneeId);

            return Result.Success();
        }

        public async Task<Result<TaskDto>> CreateTaskAsync(TaskDto taskDto)
        {
            if (string.IsNullOrWhiteSpace(taskDto.Title))
                return Result<TaskDto>.Failure("Title is required");

            logger.LogInformation("Creating new task with title: {Title}", taskDto.Title);

            var task = mapper.Map<TaskItem>(taskDto);
            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;

            await taskRepository.AddAsync(task);
            await taskRepository.SaveChangesAsync();

            logger.LogInformation("Task {TaskId} created successfully", task.Id);

            return Result<TaskDto>.Success(mapper.Map<TaskDto>(task));
        }

        public async Task<Result> DeleteTaskAsync(Guid id)
        {
            logger.LogInformation("Deleting Task {TaskId}", id);

            var task = await taskRepository.GetByIdAsync(id);

            if (task is null)
            {
                logger.LogWarning("Task {TaskId} not found for deletion", id);
                return Result.Failure("Task not found");
            }

            taskRepository.Delete(task);
            await taskRepository.SaveChangesAsync();

            logger.LogInformation("Task {TaskId} deleted", id);

            return Result.Success();
        }

        public async Task<Result<TaskDto>> GetTaskByIdAsync(Guid id)
        {
            logger.LogInformation("Fetching task {TaskId}", id);

            var task = await taskRepository.GetByIdAsync(id);

            if (task is null)
            {
                logger.LogWarning("Task {TaskId} not found", id);
                return Result<TaskDto>.Failure("Task not found");
            }

            return Result<TaskDto>.Success(mapper.Map<TaskDto>(task));
        }

        public async Task<Result<IEnumerable<TaskDto>>> GetAllTasksAsync(TaskQuery query)
        {
            logger.LogInformation("Fetching tasks with filters");

            var tasks = await taskRepository.GetAllAsync(query);
            return Result<IEnumerable<TaskDto>>.Success(mapper.Map<IEnumerable<TaskDto>>(tasks));
        }

        public async Task<Result> UpdateTaskAsync(Guid id, TaskDto taskDto)
        {
            logger.LogInformation("Updating Task {TaskId}", id);

            var task = await taskRepository.GetByIdAsync(id);
            if (task is null)
            {
                logger.LogWarning("Task {TaskId} not found for update", id);
                return Result.Failure("Task not found");
            }

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.UpdatedAt = DateTime.UtcNow;

            taskRepository.Update(task);
            await taskRepository.SaveChangesAsync();

            logger.LogInformation("Task {TaskId} updated", id);

            return Result.Success();
        }

        public async Task<Result> UpdateTaskPriorityAsync(Guid taskId, TaskPriority priority)
        {
            logger.LogInformation("Updating priority for task {TaskId} to {Priority}", taskId, priority);

            var task = await taskRepository.GetByIdAsync(taskId);

            if (task is null)
            {
                logger.LogWarning("Task {TaskId} not found for priority update", taskId);
                return Result.Failure("Task not found");
            }

            task.Priority = priority;
            taskRepository.Update(task);
            await taskRepository.SaveChangesAsync();

            logger.LogInformation("Task {TaskId} priority updated to {Priority}", taskId, priority);

            return Result.Success();
        }

        public async Task<Result> UpdateTaskStatusAsync(Guid taskId, TaskStatus status)
        {
            logger.LogInformation("Updating status for Task {TaskId} to {Status}", taskId, status);

            var task = await taskRepository.GetByIdAsync(taskId);
            if (task is null)
            {
                logger.LogWarning("Task {TaskId} not found for status update", taskId);
                return Result.Failure("Task not found");
            }

            task.Status = status;
            task.UpdatedAt = DateTime.UtcNow;

            await taskRepository.SaveChangesAsync();

            logger.LogInformation("Task {TaskId} status updated to {Status}", taskId, status);

            return Result.Success();
        }
    }
}