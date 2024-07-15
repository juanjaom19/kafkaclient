using System.Text.Json.Serialization;

namespace kafkaclient.web.Core.Dto;

public class ClusterRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("version")]
    public string? Version { get; set; }
    [JsonPropertyName("host")]
    public string? Host { get; set; }
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; } = "Activo";

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
}