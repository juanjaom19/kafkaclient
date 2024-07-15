using Confluent.Kafka;
using Confluent.Kafka.Admin;
using kafkaclient.web.Core.Dto;

namespace kafkaclient.web.Core.Services;

public class ClusterClientService
{
    private readonly ClusterService _clusterService;

    public ClusterClientService(ClusterService clusterService)
    {
        _clusterService = clusterService;
    }

    public async Task<ClusterKafkaDto> ConnectClusterAsync(int clusterId)
    {
        var clusterDb = await _clusterService.GetByIdAsync(clusterId);
        
        var adminClientConfig = new AdminClientConfig { BootstrapServers = $"{clusterDb.Host}" };
        using (var adminClient = new AdminClientBuilder(adminClientConfig).Build())
        {
            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            var brokers = metadata.Brokers.Select(b => b.Host).ToList();
            var topics = metadata.Topics.Select(t => t.Topic).ToList();
            var consumerGroups = await adminClient.ListConsumerGroupsAsync();
            // Where(group => group.State == "Stable")
            var consumers = consumerGroups.Valid.Select(group => group.GroupId)
                                            .ToList();
                                              
            return new ClusterKafkaDto
            {
                Brokers = brokers,
                Topics = topics,
                Consumers = consumers
            };
        }

    }

    public async Task<IEnumerable<PartitionDto>> GetTopicPartitionsAsync(int clusterId, string topic)
    {
        var cluster = await _clusterService.GetByIdAsync(clusterId);

        var adminClientConfig = new AdminClientConfig { BootstrapServers = cluster.Host };
        using (var adminClient = new AdminClientBuilder(adminClientConfig).Build())
        {
            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            var topicMetadata = metadata.Topics.FirstOrDefault(t => t.Topic == topic);
            if (topicMetadata == null) return null;

            return topicMetadata.Partitions.Select(p => new PartitionDto { 
                Id = p.PartitionId,
                Leader = p.Leader.ToString(),
            }).ToList();
        }
    }

    public async Task<IEnumerable<MessageDto>> GetPartitionMessagesAsync(
        int clusterId, string topic, int partition, long offset, int limit)
    {
        var cluster = await _clusterService.GetByIdAsync(clusterId);

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = cluster.Host,
            GroupId = "kafka-client-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build())
        {
            consumer.Assign(new TopicPartitionOffset(topic, new Partition(partition), new Offset(offset)));

            var messages = new List<MessageDto>();
            for (int i = 0; i < limit; i++)
            {
                var consumeResult = consumer.Consume(TimeSpan.FromSeconds(1));
                if (consumeResult != null)
                {
                    messages.Add(new MessageDto{
                        Key = consumeResult.Message.Key,
                        Offset = consumeResult.Offset.Value,
                        Value = consumeResult.Message.Value,
                        Timestamp = consumeResult.Message.Timestamp.UtcDateTime
                    });
                }
                else
                {
                    break; // No more messages to consume
                }
            }
            return messages;
        }
    }

    public async Task CreateTopicAsync(int clusterId, TopicRequest request)
    {
        var cluster = await _clusterService.GetByIdAsync(clusterId);

        var adminClientConfig = new AdminClientConfig { BootstrapServers = cluster.Host };
        using (var adminClient = new AdminClientBuilder(adminClientConfig).Build())
        {
            var topicSpecification = new TopicSpecification
            {
                Name = request.TopicName,
                NumPartitions = request.NumPartitions,
                ReplicationFactor = request.ReplicationFactor
            };

            await adminClient.CreateTopicsAsync(new List<TopicSpecification> { topicSpecification });
        }
    }

    public async Task CreateConsumerGroupAsync(int clusterId, ConsumerGroupRequest request)
    {
        // Note: Kafka does not explicitly create consumer groups.
        // A consumer group is created when the first consumer joins the group.
        // This method can be used to validate the cluster and consumer group name.

        var cluster = await _clusterService.GetByIdAsync(clusterId);

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = cluster.Host,
            GroupId = request.GroupName,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build())
        {
            // No need to consume messages, just to create the group
        }
    }

    public async Task PublishMessageAsync(int clusterId, PublishMessageRequest request)
    {
        var cluster = await _clusterService.GetByIdAsync(clusterId);

        var producerConfig = new ProducerConfig { BootstrapServers = cluster.Host };

        using (var producer = new ProducerBuilder<string, string>(producerConfig).Build())
        {
            await producer.ProduceAsync(
                request.Topic, 
                new Message<string, string> { 
                    Key = request.Key, 
                    Value = request.Value 
                }
            );
        }
    }
}