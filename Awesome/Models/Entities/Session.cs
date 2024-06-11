namespace Awesome.Models.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string Token { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public required User User { get; set; }
    }
}
