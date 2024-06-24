using System.Text.Json.Serialization;

namespace AwesomeUI.Model;

public class Blog
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    [JsonPropertyName("title")]
    public required string Title { get; set; }
    
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    
    [JsonPropertyName("thumbnail")]

    public required string Thumbnail { get; set; }
    
    [JsonPropertyName("author")]
    public required string Author { get; set; }
    
    [JsonPropertyName("content")]
    public required string Content { get; set; }
    
    [JsonPropertyName("categories")]
    public required Category[] Categories { get; set; }
    
    [JsonPropertyName("createdAt")]
    public required DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

[JsonSerializable(typeof(List<Blog>))]
internal sealed partial class BlogContext : JsonSerializerContext
{
    
}