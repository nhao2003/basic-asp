namespace Awesome.Models.Entities
{
    public class Blog
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        public required string Thumbnail { get; set; }
        public required string Author { get; set; }
        public required string Content { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public ICollection<Category> Categories { get; set; }
    }
}
