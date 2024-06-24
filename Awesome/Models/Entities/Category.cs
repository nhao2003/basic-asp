namespace Awesome.Models.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<Blog> Blogs { get; set; }
}