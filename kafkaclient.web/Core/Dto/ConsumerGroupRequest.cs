using System.Text.Json.Serialization;

namespace kafkaclient.web.Core.Dto;

public class ConsumerGroupRequest
{
    [JsonPropertyName("group_name")]
    public string GroupName { get; set; }
}