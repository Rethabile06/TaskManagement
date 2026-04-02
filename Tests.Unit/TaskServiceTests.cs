using AutoMapper;
using Core.Entities;
using Core.Interfaces.IRepositories;
using Core.Models;
using Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using TaskStatus = Core.Enums.TaskStatus;

namespace Tests.Unit
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepository;
        private readonly Mock<ITeamMemberRepository> _memberRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<TaskService>> _logger;

        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepository = new Mock<ITaskRepository>();
            _memberRepository = new Mock<ITeamMemberRepository>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<TaskService>>();

            _taskService = new TaskService(_taskRepository.Object, _memberRepository.Object, _mapper.Object, _logger.Object);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldReturnFailure_WhenTitleIsEmpty()
        {
            // Arrange
            var taskDto = new TaskDto { Title = "" };

            // Act
            var result = await _taskService.CreateTaskAsync(taskDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Title is required", result.Error);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldReturnSuccess_WhenTitleIsValid()
        {
            // Arrange
            var taskDto = new TaskDto { Title = "Complete Assessment" };
            var taskEntity = new TaskItem { Title = "Complete Assessment" };

            _mapper.Setup(x => x.Map<TaskItem>(taskDto)).Returns(taskEntity);
            _mapper.Setup(x => x.Map<TaskDto>(taskEntity)).Returns(taskDto);

            // Act
            var result = await _taskService.CreateTaskAsync(taskDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);

            _taskRepository.Verify(x => x.AddAsync(It.IsAny<TaskItem>()), Times.Once);
            _taskRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AssignTaskAsync_ShouldReturnSuccess_WhenTaskAndMemberExist()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var memberId = Guid.NewGuid();

            var existingTask = new TaskItem { Id = taskId, Title = "Test Task" };
            var existingMember = new TeamMember { Id = memberId, Name = "John", Surname = "Snow" };

            _taskRepository.Setup(x => x.GetByIdAsync(taskId))
                .ReturnsAsync(existingTask);

            _memberRepository.Setup(x => x.GetByIdAsync(memberId))
                .ReturnsAsync(existingMember);

            // Act
            var result = await _taskService.AssignTaskAsync(taskId, memberId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(memberId, existingTask.AssigneeId);

            _taskRepository.Verify(x => x.Update(existingTask), Times.Once);
            _taskRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AssignTaskAsync_ShouldReturnFailure_WhenTaskDoesNotExist()
        {
            // Arrange
            _taskRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TaskItem?)null);

            // Act
            var result = await _taskService.AssignTaskAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Task not found", result.Error);
        }

        [Fact]
        public async Task UpdateTaskStatusAsync_ShouldUpdateStatus_AndCallRepository()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskItem { Id = taskId, Title = "Title", Status = TaskStatus.Todo };

            _taskRepository.Setup(x => x.GetByIdAsync(taskId))
                .ReturnsAsync(task);

            // Act
            var result = await _taskService.UpdateTaskStatusAsync(taskId, TaskStatus.InProgress);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(TaskStatus.InProgress, task.Status);

            _taskRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}