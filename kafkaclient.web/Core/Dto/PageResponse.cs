using System.Text.Json.Serialization;

namespace kafkaclient.web.Core.Dto;

public class PageResponse<T>
{
    [JsonPropertyName("count")]
    public int? Count { get; set; }
    [JsonPropertyName("offset")]
    public int? Offset { get; set; }
    [JsonPropertyName("limit")]
    public int? Limit { get; set; }
    [JsonPropertyName("total")]
    public int? Total { get; set; }
    [JsonPropertyName("data")]
    public List<T>? Data { get; set; }
}