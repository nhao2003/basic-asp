using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.Blog
{
    public class CreateBlogDto
    {
        [Required]
        [StringLength(255)]
        public required string Title { get; init; }
        [Required]
        [DataType(DataType.MultilineText)]
        public required string Description { get; init; }

        [Required]
        [Url]
        public required string Thumbnail { get; init; }

        [Required]
        [StringLength(255)]
        public required string Author { get; init; }
        [Required]
        [DataType(DataType.MultilineText)]
        public required string Content { get; init; }

        public required Guid[] Categories { get; init; }
    }
}
