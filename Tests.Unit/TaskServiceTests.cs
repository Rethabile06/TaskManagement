using AutoMapper;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.IRepositories;
using Core.Models.Task;
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
            var request = new CreateTaskRequest { Title = "" };

            // Act
            var result = await _taskService.CreateTaskAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Title is required", result.Error);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldReturnSuccess_WhenTitleIsValid()
        {
            // Arrange
            var request = new CreateTaskRequest { Title = "Complete Assessment" };
            var Response = new TaskResponse { Title = "Complete Assessment" };
            var taskEntity = new TaskItem { Title = "Complete Assessment" };

            _mapper.Setup(x => x.Map<TaskItem>(request)).Returns(taskEntity);
            _mapper.Setup(x => x.Map<TaskResponse>(taskEntity)).Returns(Response);

            // Act
            var result = await _taskService.CreateTaskAsync(request);

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

            var task = new TaskItem { Id = taskId, Title = "Test Task" };
            var member = new TeamMember { Id = memberId, Name = "John", Surname = "Snow" };

            _taskRepository.Setup(x => x.GetByIdAsync(taskId))
                .ReturnsAsync(task);

            _memberRepository.Setup(x => x.GetByIdAsync(memberId))
                .ReturnsAsync(member);

            // Act
            var result = await _taskService.AssignTaskAsync(taskId, memberId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(memberId, task.MemberId);

            _taskRepository.Verify(x => x.Update(task), Times.Once);
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
        public async Task UpdateTaskAsync_ShouldReturnSuccess_WhenTaskAndMemberExist()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var memberId = Guid.NewGuid();

            var request = new UpdateTaskRequest { Description = "Updated Description", Status = TaskStatus.InProgress };
            var task = new TaskItem { Id = taskId, Title = "Title", Description = "Description", Status = TaskStatus.Todo, Priority = TaskPriority.Low };
            var member = new TeamMember { Id = memberId, Name = "Arya", Surname = "Stark" };

            _taskRepository.Setup(x => x.GetByIdAsync(taskId))
                .ReturnsAsync(task);

            _memberRepository.Setup(r => r.GetByIdAsync(memberId))
                .ReturnsAsync(member);

            // Act
            var result = await _taskService.UpdateTaskAsync(taskId, request);

            // Assert
            Assert.True(result.IsSuccess);

            _mapper.Verify(m => m.Map(request, task), Times.Once);
            _taskRepository.Verify(r => r.Update(task), Times.Once);
            _taskRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldReturnFailure_WhenTaskDoesNotExist()
        {
            // Arrange
            _taskRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TaskItem?)null);

            // Act
            var result = await _taskService.UpdateTaskAsync(Guid.NewGuid(), new UpdateTaskRequest());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Task not found", result.Error);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldReturnFailure_WhenMemberIsProvidedButDoesNotExist()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var memberId = Guid.NewGuid();

            var request = new UpdateTaskRequest { MemberId = memberId };
            var task = new TaskItem { Id = taskId, Title = "Title" };

            _taskRepository.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);
            _memberRepository.Setup(r => r.GetByIdAsync(memberId)).ReturnsAsync((TeamMember?)null);

            // Act
            var result = await _taskService.UpdateTaskAsync(taskId, request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Team member not found", result.Error);
        }
    }
}