namespace Awesome.Models.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public User User { get; set; }
    }
}
