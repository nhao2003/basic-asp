using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.Blog
{
    public class UpdateBlogDto
    {

        [StringLength(255)]
        public string? Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Url]
        public string? Thumbnail { get; set; }

        [StringLength(255)]
        public string? Author { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Content { get; set; }
    }
}
