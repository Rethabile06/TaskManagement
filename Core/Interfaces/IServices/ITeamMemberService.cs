using Core.Common;
using Core.Models.TeamMember;

namespace Core.Interfaces.IServices
{
    public interface ITeamMemberService
    {
        Task<Result<IEnumerable<TeamMemberResponse>>> GetAllMembersAsync();
        Task<Result<TeamMemberResponse>> GetMemberByIdAsync(Guid id);
        Task<Result<TeamMemberResponse>> CreateMemberAsync(TeamMemberRequest request);
        Task<Result> UpdateMemberAsync(Guid id, TeamMemberRequest request);
        Task<Result> DeleteMemberAsync(Guid id);
    }
}