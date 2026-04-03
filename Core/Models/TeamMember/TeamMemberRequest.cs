using System.ComponentModel.DataAnnotations;

namespace Core.Models.TeamMember
{
    public class TeamMemberRequest
    {
        [Required(ErrorMessage = "First name is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        public required string Surname { get; set; }
    }
}