using kafkaclient.web.Core.Dto;
using kafkaclient.web.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace kafkaclient.web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClusterClientApiController : ControllerBase
{
    private readonly ClusterClientService _clusterClientService;
    public ClusterClientApiController(ClusterClientService clusterClientService)
    {
        _clusterClientService = clusterClientService;
    }


    [HttpPost("{id}/connect")]
    [ProducesResponseType(typeof(ClusterKafkaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ConnectCluster(int id)
    {
        var clusterKafkaInfo = await _clusterClientService.ConnectClusterAsync(id);
        return Ok(clusterKafkaInfo);
    }

    [HttpGet("{id}/topics/{topic}/partitions")]
    public async Task<ActionResult<IEnumerable<PartitionDto>>> GetTopicPartitions(int id, string topic)
    {
        var partitions = await _clusterClientService.GetTopicPartitionsAsync(id, topic);
        return Ok(partitions);
    }

    [HttpGet("{id}/topics/{topic}/partitions/{partition}/messages")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetPartitionMessages(
        int id, string topic, int partition, [FromQuery] long offset, [FromQuery] int limit = 10)
    {
        var messages = await _clusterClientService.GetPartitionMessagesAsync(id, topic, partition, offset, limit);
        if (messages == null)
        {
            return NotFound();
        }
        return Ok(messages);
    }

    [HttpPost("{id}/topics")]
    public async Task<IActionResult> CreateTopic(int id, [FromBody]TopicRequest request)
    {
        await _clusterClientService.CreateTopicAsync(id, request);
        return Ok();
    }

    [HttpPost("{id}/consumergroups")]
    public async Task<IActionResult> CreateConsumerGroup(int id, [FromBody]ConsumerGroupRequest request)
    {
        await _clusterClientService.CreateConsumerGroupAsync(id, request);
        return Ok();
    }

    [HttpPost("{id}/messages")]
    public async Task<IActionResult> PublishMessage(int id, [FromBody]PublishMessageRequest request)
    {
        await _clusterClientService.PublishMessageAsync(id, request);
        return Ok();
    }
}