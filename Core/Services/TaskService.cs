using AutoMapper;
using Core.Common;
using Core.Entities;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices;
using Core.Models.Task;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class TaskService(ITaskRepository taskRepository, ITeamMemberRepository memberRepository, IMapper mapper, ILogger<TaskService> logger) : ITaskService
    {
        public async Task<Result> AssignTaskAsync(Guid taskId, Guid memberId)
        {
            logger.LogInformation("Assigning task {TaskId} to member {MemberId}", taskId, memberId);

            var task = await taskRepository.GetByIdAsync(taskId);
            if (task is null)
            {
                logger.LogWarning("Task {TaskId} not found", taskId);
                return Result.Failure("Task not found");
            }

            var member = await memberRepository.GetByIdAsync(memberId);
            if (member is null)
            {
                logger.LogWarning("Team member {MemberId} not found", memberId);
                return Result.Failure("Team member not found");
            }

            task.MemberId = member.Id;
            task.UpdatedAt = DateTime.UtcNow;

            taskRepository.Update(task);
            await taskRepository.SaveChangesAsync();

            logger.LogInformation("Task {TaskId} assigned to member {MemberId}", taskId, memberId);

            return Result.Success();
        }

        public async Task<Result<TaskResponse>> CreateTaskAsync(CreateTaskRequest request)
        {
            logger.LogInformation("Creating new task with title: {Title}", request.Title);

            if (string.IsNullOrWhiteSpace(request.Title))
                return Result<TaskResponse>.Failure("Title is required");

            if (request.MemberId.HasValue)
            {
                var member = await memberRepository.GetByIdAsync(request.MemberId.Value);
                if (member is null)
                {
                    logger.LogWarning("Team member {MemberId} not found", request.MemberId.Value);
                    return Result<TaskResponse>.Failure("Team member not found");
                }
            }

            var task = mapper.Map<TaskItem>(request);
            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;

            await taskRepository.AddAsync(task);
            await taskRepository.SaveChangesAsync();

            logger.LogInformation("Task {TaskId} created successfully", task.Id);

            return Result<TaskResponse>.Success(mapper.Map<TaskResponse>(task));
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

        public async Task<Result<TaskResponse>> GetTaskByIdAsync(Guid id)
        {
            logger.LogInformation("Fetching task {TaskId}", id);

            var task = await taskRepository.GetByIdAsync(id);

            if (task is null)
            {
                logger.LogWarning("Task {TaskId} not found", id);
                return Result<TaskResponse>.Failure("Task not found");
            }

            return Result<TaskResponse>.Success(mapper.Map<TaskResponse>(task));
        }

        public async Task<Result<IEnumerable<TaskResponse>>> GetAllTasksAsync(TaskQuery query)
        {
            logger.LogInformation("Fetching tasks with filters");

            var tasks = await taskRepository.GetAllAsync(query.Status, query.Priority, query.MemberId, query.SearchTerm);
            return Result<IEnumerable<TaskResponse>>.Success(mapper.Map<IEnumerable<TaskResponse>>(tasks));
        }

        public async Task<Result> UpdateTaskAsync(Guid id, UpdateTaskRequest request)
        {
            logger.LogInformation("Updating Task {TaskId}", id);

            var task = await taskRepository.GetByIdAsync(id);
            if (task is null)
            {
                logger.LogWarning("Task {TaskId} not found for update", id);
                return Result.Failure("Task not found");
            }

            if (request.MemberId.HasValue)
            {
                var member = await memberRepository.GetByIdAsync(request.MemberId.Value);
                if (member is null)
                {
                    logger.LogWarning("Team member {MemberId} not found", request.MemberId.Value);
                    return Result.Failure("Team member not found");
                }
            }

            mapper.Map(request, task);
            task.UpdatedAt = DateTime.UtcNow;

            taskRepository.Update(task);
            await taskRepository.SaveChangesAsync();

            logger.LogInformation("Task {TaskId} updated", id);

            return Result.Success();
        }
    }
}