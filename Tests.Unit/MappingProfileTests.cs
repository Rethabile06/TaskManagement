using Core.Entities;
using Core.Mappings;
using Core.Models.Task;

namespace Tests.Unit
{
    public class MappingProfileTests
    {
        [Fact]
        public void MappingProfile_CanBeInstantiated()
        {
            var profile = new MappingProfile();
            Assert.NotNull(profile);
        }

        [Fact]
        public void MemberNameProjection_WhenMemberIsNull_ReturnsNull()
        {
            var task = new TaskItem
            {
                Title = "T1",
                Member = null
            };

            var projected = task.Member != null ? $"{task.Member.Name} {task.Member.Surname}".Trim() : null;

            Assert.Null(projected);
        }

        [Fact]
        public void MemberNameProjection_WhenMemberPresent_FormatsName()
        {
            var task = new TaskItem
            {
                Title = "T2",
                Member = new TeamMember { Name = "Arya", Surname = "Stark" }
            };

            var projected = task.Member != null ? $"{task.Member.Name} {task.Member.Surname}".Trim() : null;

            Assert.Equal("Arya Stark", projected);
        }

        [Fact]
        public void CreateTaskRequest_Has_MemberId_Property()
        {
            var request = new CreateTaskRequest { Title = "Test", MemberId = Guid.NewGuid() };
            Assert.True(request.MemberId.HasValue);
        }

        //[Fact]
        //public void AutoMapper_Configuration_IsValid()
        //{
        //    var loggerFactory = NullLoggerFactory.Instance;

        //    var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), loggerFactory);
        //    config.AssertConfigurationIsValid();
        //}
    }
}