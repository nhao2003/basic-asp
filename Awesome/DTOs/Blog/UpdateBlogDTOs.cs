using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.Blog
{
    public class UpdateBlogDTO
    {

        [StringLength(255)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Thumbnail { get; set; }

        [StringLength(255)]
        public string? Author { get; set; }

        public string? Content { get; set; }
    }
}
