namespace DevFlow.Identity.Entities
{
    public class User
    {
        public int Id { get; set; } 

        public string UserName { get; set; }
        public int TenantId { get; set; }

        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
