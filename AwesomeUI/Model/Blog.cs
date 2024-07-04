using System.Text.Json.Serialization;
using SQLite;

namespace AwesomeUI.Model;

public class Blog
{
    [JsonPropertyName("id")] [PrimaryKey] public string? Id { get; set; }

    [JsonPropertyName("title")] public string Title { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("thumbnail")] public string Thumbnail { get; set; }

    [JsonPropertyName("author")] public string Author { get; set; }

    [JsonPropertyName("content")] public string Content { get; set; }
    
    [JsonPropertyName("categories")]
    [Ignore]
    public string[]? Categories { get; set; }
    
    [JsonIgnore]
    public string CategoriesString
    {
        get => Categories == null ? string.Empty : string.Join(",", Categories);
        set => Categories = value.Split(',');
    }
    
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")] public DateTime? UpdatedAt { get; set; }

    public Blog()
    {
    }
}

[JsonSerializable(typeof(List<Blog>))]
internal sealed partial class BlogContext : JsonSerializerContext
{
}