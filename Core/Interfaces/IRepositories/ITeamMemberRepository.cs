using Core.Entities;

namespace Core.Interfaces.IRepositories
{
    public interface ITeamMemberRepository
    {
        Task<TeamMember?> GetByIdAsync(Guid id);
        Task<IEnumerable<TeamMember>> GetAllAsync();
        Task AddAsync(TeamMember member);
        void Update(TeamMember member);
        void Delete(TeamMember member);
        Task SaveChangesAsync();
    }
}