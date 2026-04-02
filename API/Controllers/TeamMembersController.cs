using Core.Interfaces.IServices;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMembersController(ITeamMemberService memberService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllMembers()
        {
            var result = await memberService.GetAllMembersAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(Guid id)
        {
            var result = await memberService.GetMemberByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(TeamMemberDto memberDto)
        {
            var result = await memberService.CreateMemberAsync(memberDto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetMemberById), new { id = result.Data?.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(Guid id, TeamMemberDto memberDto)
        {
            var result = await memberService.UpdateMemberAsync(id, memberDto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var result = await memberService.DeleteMemberAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return NoContent();
        }
    }
}