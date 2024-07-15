using System.Text.Json.Serialization;

namespace kafkaclient.web.Core.Dto;

public class PublishMessageRequest
{
    [JsonPropertyName("topic")]
    public string Topic { get; set; }
    [JsonPropertyName("key")]
    public string Key { get; set; }
    [JsonPropertyName("value")]
    public string Value { get; set; }
}