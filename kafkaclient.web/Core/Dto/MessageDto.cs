using System.Text.Json.Serialization;

namespace kafkaclient.web.Core.Dto;

public class MessageDto
{
    [JsonPropertyName("offset")]
    public long Offset { get; set; }
    [JsonPropertyName("key")]
    public string Key { get; set; }
    [JsonPropertyName("value")]
    public string Value { get; set; }
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } 
}