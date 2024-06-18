namespace Awesome.DTOs.Blog;

public class BlogResponseDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Thumbnail { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}