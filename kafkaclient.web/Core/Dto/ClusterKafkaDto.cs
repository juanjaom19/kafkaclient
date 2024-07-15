using System.Text.Json.Serialization;

namespace kafkaclient.web.Core.Dto;

public class ClusterKafkaDto
{
    [JsonPropertyName("brokers")]
    public List<string> Brokers { get; set; }
    [JsonPropertyName("topics")]
    public List<string> Topics { get; set; }
    [JsonPropertyName("consumers")]
    public List<string> Consumers { get; set; }
}