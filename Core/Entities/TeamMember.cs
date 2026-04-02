namespace Core.Entities
{
    public class TeamMember : BaseEntity
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
    }
}