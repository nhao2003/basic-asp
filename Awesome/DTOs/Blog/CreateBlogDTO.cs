using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.Blog
{
    public class CreateBlogDto
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Url]
        public string Thumbnail { get; set; }

        [Required]
        [StringLength(255)]
        public string Author { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}
