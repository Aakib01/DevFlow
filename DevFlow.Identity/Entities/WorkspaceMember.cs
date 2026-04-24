namespace DevFlow.Identity.Entities
{
    public class WorkspaceMember
    {
        public int Id { get; set; } 

        public int WorkspaceId { get; set; }
        public int UserId { get; set; }

        public string Role { get; set; } = "Member";
    }
}
