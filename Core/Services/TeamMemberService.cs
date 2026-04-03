using AutoMapper;
using Core.Common;
using Core.Entities;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices;
using Core.Models.TeamMember;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class TeamMemberService(ITeamMemberRepository memberRepository, IMapper mapper, ILogger<TeamMemberService> logger) : ITeamMemberService
    {
        public async Task<Result<TeamMemberResponse>> CreateMemberAsync(TeamMemberRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Surname))
                return Result<TeamMemberResponse>.Failure("Member name and surname is required");

            logger.LogInformation("Creating new team member: {Name} {Surname}", request.Name, request.Surname);

            var member = mapper.Map<TeamMember>(request);
            member.Id = Guid.NewGuid();
            member.CreatedAt = DateTime.UtcNow;

            await memberRepository.AddAsync(member);
            await memberRepository.SaveChangesAsync();

            return Result<TeamMemberResponse>.Success(mapper.Map<TeamMemberResponse>(member));
        }

        public async Task<Result> DeleteMemberAsync(Guid id)
        {
            logger.LogInformation("Deleting team member {MemberId}", id);

            var member = await memberRepository.GetByIdAsync(id);
            if (member is null)
            {
                logger.LogWarning("Delete failed: Member {MemberId} not found", id);
                return Result.Failure("Team member not found");
            }

            memberRepository.Delete(member);
            await memberRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<TeamMemberResponse>>> GetAllMembersAsync()
        {
            logger.LogInformation("Fetching all team members.");

            var members = await memberRepository.GetAllAsync();

            return Result<IEnumerable<TeamMemberResponse>>.Success(mapper.Map<IEnumerable<TeamMemberResponse>>(members));
        }

        public async Task<Result<TeamMemberResponse>> GetMemberByIdAsync(Guid id)
        {
            logger.LogInformation("Fetching team member {MemberId}", id);

            var member = await memberRepository.GetByIdAsync(id);
            if (member is null)
            {
                logger.LogWarning("Team member {MemberId} not found", id);
                return Result<TeamMemberResponse>.Failure("Team member not found");
            }

            return Result<TeamMemberResponse>.Success(mapper.Map<TeamMemberResponse>(member));
        }

        public async Task<Result> UpdateMemberAsync(Guid id, TeamMemberRequest request)
        {
            logger.LogInformation("Updating team member {MemberId}", id);

            var member = await memberRepository.GetByIdAsync(id);
            if (member is null)
            {
                logger.LogWarning("Update failed: Member {MemberId} not found", id);
                return Result.Failure("Team member not found");
            }

            mapper.Map(request, member);
            member.UpdatedAt = DateTime.UtcNow;

            memberRepository.Update(member);
            await memberRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}