using AutoMapper;
using Core.Common;
using Core.Entities;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class TeamMemberService(ITeamMemberRepository memberRepository, IMapper mapper, ILogger<TeamMemberService> logger) : ITeamMemberService
    {
        public async Task<Result<TeamMemberDto>> CreateMemberAsync(TeamMemberDto memberDto)
        {
            if (string.IsNullOrWhiteSpace(memberDto.Name) || string.IsNullOrWhiteSpace(memberDto.Surname))
                return Result<TeamMemberDto>.Failure("Member name and surname is required");

            logger.LogInformation("Creating new team member: {Name} {Surname}", memberDto.Name, memberDto.Surname);

            var member = mapper.Map<TeamMember>(memberDto);
            member.Id = Guid.NewGuid();
            member.CreatedAt = DateTime.UtcNow;

            await memberRepository.AddAsync(member);
            await memberRepository.SaveChangesAsync();

            return Result<TeamMemberDto>.Success(mapper.Map<TeamMemberDto>(member));
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

        public async Task<Result<IEnumerable<TeamMemberDto>>> GetAllMembersAsync()
        {
            logger.LogInformation("Fetching all team members.");

            var members = await memberRepository.GetAllAsync();

            return Result<IEnumerable<TeamMemberDto>>.Success(mapper.Map<IEnumerable<TeamMemberDto>>(members));
        }

        public async Task<Result<TeamMemberDto>> GetMemberByIdAsync(Guid id)
        {
            logger.LogInformation("Fetching team member {MemberId}", id);

            var member = await memberRepository.GetByIdAsync(id);
            if (member is null)
            {
                logger.LogWarning("Team member {MemberId} not found", id);
                return Result<TeamMemberDto>.Failure("Team member not found");
            }

            return Result<TeamMemberDto>.Success(mapper.Map<TeamMemberDto>(member));
        }

        public async Task<Result> UpdateMemberAsync(Guid id, TeamMemberDto memberDto)
        {
            logger.LogInformation("Updating team member {MemberId}", id);

            var member = await memberRepository.GetByIdAsync(id);
            if (member is null)
            {
                logger.LogWarning("Update failed: Member {MemberId} not found", id);
                return Result.Failure("Team member not found");
            }

            // Map updated values from DTO to the tracked entity
            mapper.Map(memberDto, member);
            member.UpdatedAt = DateTime.UtcNow;

            memberRepository.Update(member);
            await memberRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}