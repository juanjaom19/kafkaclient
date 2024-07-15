using System.Text.Json.Serialization;

namespace kafkaclient.web.Core.Dto;

public class TopicRequest
{
    [JsonPropertyName("topic_name")]
    public string TopicName { get; set; }
    [JsonPropertyName("num_partitions")]
    public int NumPartitions { get; set; }
    [JsonPropertyName("replication_factor")]
    public short ReplicationFactor { get; set; }
}