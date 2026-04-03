using Core.Interfaces.IServices;
using Core.Models.Task;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> CreateTask(CreateTaskRequest request)
        {
            var result = await taskService.CreateTaskAsync(request);

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
        public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskRequest request)
        {
            var result = await taskService.UpdateTaskAsync(id, request);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }

        [HttpPut("{id}/assign/{memberId}")]
        public async Task<IActionResult> AssignTask(Guid id, Guid memberId)
        {
            var result = await taskService.AssignTaskAsync(id, memberId);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
}