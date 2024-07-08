using Awesome.DTOs.Category;

namespace Awesome.DTOs.Blog;

public class BlogResponseDto
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Thumbnail { get; init; }
    public required string Author { get; init; }
    public required string Content { get; init; }
    public required List<string> Categories { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}