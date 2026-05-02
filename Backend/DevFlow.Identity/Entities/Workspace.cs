namespace DevFlow.Identity.Entities
{
    public class Workspace
    {
        public int Id { get; set; } 
        public int TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
