using System.Text.Json.Serialization;

namespace kafkaclient.web.Core.Dto;

public class ClusterDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("version")]
    public string? Version { get; set; }
    [JsonPropertyName("host")]
    public string? Host { get; set; }
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("status")]
    public bool Status { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
}