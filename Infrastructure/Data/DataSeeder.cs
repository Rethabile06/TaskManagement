using Core.Entities;
using Core.Enums;
using Microsoft.Extensions.Logging;
using TaskStatus = Core.Enums.TaskStatus;

namespace Infrastructure.Data
{
    public class DataSeeder
    {
        public static async Task SeedAsync(TaskDbContext context, ILogger logger)
        {
            if (context.Tasks.Any())
            {
                logger.LogInformation("Database already seeded.");
                return;
            }

            logger.LogInformation("Seeding database.");

            var users = new List<TeamMember>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "John",
                        Surname = "Snow"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Arya",
                        Surname = "Stark"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Tyrion",
                        Surname = "Lannister"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Lord",
                        Surname = "Varys"
                    },
                };

            context.TeamMembers.AddRange(users);

            var tasks = new List<TaskItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Investigation Bugs",
                        Description = "Investigate Backend Services.",
                        Priority = TaskPriority.Medium,
                        Status = TaskStatus.Todo,
                        CreatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Onboarding",
                        Description = "Onboard New Team Memebers.",
                        Priority = TaskPriority.Low,
                        Status = TaskStatus.Todo,
                        AssigneeId = users.FirstOrDefault()?.Id,
                        CreatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Finish Assessment",
                        Description = "Implement Clean Architecture.",
                        Priority = TaskPriority.High,
                        Status = TaskStatus.InProgress,
                        AssigneeId = users.LastOrDefault()?.Id,
                        CreatedAt = DateTime.UtcNow
                    }
                };

            context.Tasks.AddRange(tasks);
            await context.SaveChangesAsync();

            logger.LogInformation("Database seeding completed.");

        }
    }
}