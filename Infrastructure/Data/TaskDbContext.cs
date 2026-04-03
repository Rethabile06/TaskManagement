using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class TaskDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
    }
}