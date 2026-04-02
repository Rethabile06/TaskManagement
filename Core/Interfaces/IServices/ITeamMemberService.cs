using Core.Common;
using Core.Models;

namespace Core.Interfaces.IServices
{
    public interface ITeamMemberService
    {
        Task<Result<IEnumerable<TeamMemberDto>>> GetAllMembersAsync();
        Task<Result<TeamMemberDto>> GetMemberByIdAsync(Guid id);
        Task<Result<TeamMemberDto>> CreateMemberAsync(TeamMemberDto memberDto);
        Task<Result> UpdateMemberAsync(Guid id, TeamMemberDto memberDto);
        Task<Result> DeleteMemberAsync(Guid id);
    }
}