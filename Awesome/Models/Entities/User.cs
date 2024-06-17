namespace Awesome.Models.Entities
{
    public enum UserRole
    {
        Admin,
        User
    }
    public class User
    {
        public Guid Id { get; set; }
        
        public UserRole Role { get; set; } = UserRole.User;
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Session> Sessions { get; set; }
    }
}
