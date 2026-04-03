using AutoMapper;
using Core.Entities;
using Core.Interfaces.IRepositories;
using Core.Models.TeamMember;
using Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Unit
{
    public class TeamMemberServiceTests
    {
        private readonly Mock<ITeamMemberRepository> _memberRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<TeamMemberService>> _logger;

        private readonly TeamMemberService _memberService;

        public TeamMemberServiceTests()
        {
            _memberRepository = new Mock<ITeamMemberRepository>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<TeamMemberService>>();

            _memberService = new TeamMemberService(_memberRepository.Object, _mapper.Object, _logger.Object);
        }

        [Fact]
        public async Task CreateMemberAsync_ShouldReturnSuccess_WhenDataIsValid()
        {
            // Arrange
            var request = new TeamMemberRequest { Name = "Tyrion", Surname = "Lannister" };
            var response = new TeamMemberResponse { Name = "Tyrion", Surname = "Lannister" };
            var memberEntity = new TeamMember { Name = "Tyrion", Surname = "Lannister" };

            _mapper.Setup(m => m.Map<TeamMember>(request))
                .Returns(memberEntity);

            _mapper.Setup(m => m.Map<TeamMemberResponse>(memberEntity))
                .Returns(response);

            // Act
            var result = await _memberService.CreateMemberAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);

            _memberRepository.Verify(x => x.AddAsync(It.IsAny<TeamMember>()), Times.Once);
            _memberRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData("", "Snow")]
        [InlineData("Jon", "")]
        [InlineData(null, null)]
        public async Task CreateMemberAsync_ShouldReturnFailure_WhenNameOrSurnameIsMissing(string name, string surname)
        {
            // Arrange
            var request = new TeamMemberRequest { Name = name, Surname = surname };

            // Act
            var result = await _memberService.CreateMemberAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Member name and surname is required", result.Error);
        }

        [Fact]
        public async Task GetMemberByIdAsync_ShouldReturnFailure_WhenMemberNotFound()
        {
            // Arrange
            _memberRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TeamMember?)null);

            // Act
            var result = await _memberService.GetMemberByIdAsync(Guid.NewGuid());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Team member not found", result.Error);
        }

        [Fact]
        public async Task UpdateMemberAsync_ShouldSetUpdatedAt_AndCallRepository()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            var member = new TeamMember { Id = memberId, Name = "Old", Surname = "Name" };
            var request = new TeamMemberRequest { Name = "New", Surname = "Name" };

            _memberRepository.Setup(x => x.GetByIdAsync(memberId))
                .ReturnsAsync(member);

            // Act
            var result = await _memberService.UpdateMemberAsync(memberId, request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(DateTime.UtcNow, member.UpdatedAt, TimeSpan.FromSeconds(1));

            _mapper.Verify(m => m.Map(request, member), Times.Once);
            _memberRepository.Verify(x => x.Update(member), Times.Once);
            _memberRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteMemberAsync_ShouldReturnSuccess_WhenMemberExists()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            var member = new TeamMember { Id = memberId, Name = "Arya", Surname = "Stark" };

            _memberRepository.Setup(x => x.GetByIdAsync(memberId))
                .ReturnsAsync(member);

            // Act
            var result = await _memberService.DeleteMemberAsync(memberId);

            // Assert
            Assert.True(result.IsSuccess);

            _memberRepository.Verify(x => x.Delete(member), Times.Once);
            _memberRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}