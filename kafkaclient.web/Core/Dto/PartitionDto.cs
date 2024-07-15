using System.Text.Json.Serialization;

namespace kafkaclient.web.Core.Dto;

public class PartitionDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("leader")]
    public string Leader { get; set; }
}