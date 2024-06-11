using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs
{
    public class CreateBlogDTO
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public string Thumbnail { get; set; }

        [Required]
        [StringLength(255)]
        public string Author { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
