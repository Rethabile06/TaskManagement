using Core.Entities;
using Core.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TeamMemberRepository(TaskDbContext context) : ITeamMemberRepository
    {
        public async Task AddAsync(TeamMember member)
        {
            await context.TeamMembers.AddAsync(member);
        }

        public void Delete(TeamMember member)
        {
            context.Remove(member);
        }

        public async Task<IEnumerable<TeamMember>> GetAllAsync()
        {
            return await context.TeamMembers.ToListAsync();
        }

        public async Task<TeamMember?> GetByIdAsync(Guid id)
        {
            return await context.TeamMembers.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Update(TeamMember member)
        {
            context.Entry(member).Property(m => m.CreatedAt).IsModified = false;
            context.Entry(member).State = EntityState.Modified;
        }
    }
}