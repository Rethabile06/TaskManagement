using Core.Enums;
using Core.Interfaces.IServices;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using TaskStatus = Core.Enums.TaskStatus;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController(ITaskService taskService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllTasks([FromQuery] TaskQuery query)
        {
            var result = await taskService.GetAllTasksAsync(query);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var result = await taskService.GetTaskByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskDto taskDto)
        {
            var result = await taskService.CreateTaskAsync(taskDto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetTaskById), new { id = result.Data?.Id }, result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var result = await taskService.DeleteTaskAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, TaskDto dto)
        {
            var result = await taskService.UpdateTaskAsync(id, dto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }

        [HttpPut("{id}/assign/{assigneeId}")]
        //public async Task<IActionResult> AssignTask(Guid id, [FromBody] Guid assigneeId)
        public async Task<IActionResult> AssignTask(Guid id, Guid assigneeId)
        {
            var result = await taskService.AssignTaskAsync(id, assigneeId);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }

        [HttpPut("{id}/priority")]
        public async Task<IActionResult> UpdateTaskPriority(Guid id, [FromBody] TaskPriority priority)
        {
            var result = await taskService.UpdateTaskPriorityAsync(id, priority);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(Guid id, [FromBody] TaskStatus status)
        {
            var result = await taskService.UpdateTaskStatusAsync(id, status);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
}