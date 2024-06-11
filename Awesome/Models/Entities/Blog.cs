namespace Awesome.Models.Entities
{
    public class Blog
    {
        /// <summary>
        /// Guid is a 128-bit integer that is globally unique.
        /// This is used to uniquely identify the blog.
        /// </summary>
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        public required string Thumbnail { get; set; }
        public required string Author { get; set; }
        public required string Content { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
