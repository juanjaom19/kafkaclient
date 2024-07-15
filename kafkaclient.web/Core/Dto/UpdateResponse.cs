using System.Text.Json.Serialization;

namespace kafkaclient.web.Core.Dto;

public class UpdateResponse<T>
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}