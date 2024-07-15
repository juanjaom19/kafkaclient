using kafkaclient.web.Core.Dto;
using kafkaclient.web.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace kafkaclient.web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClusterApiController : ControllerBase
{
    private readonly ClusterService _clusterService;

    public ClusterApiController(ClusterService clusterService)
    {
        _clusterService = clusterService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PageResponse<ClusterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery]PagingRequest query_paging,
        [FromQuery]ClusterFilterRequest query_filter,
        [FromQuery]ClusterOrderByRequest? query_order_by)
    {
        var data = await _clusterService.GetAllAsync(query_paging, query_filter, query_order_by);
        return Ok(data);
    } 

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClusterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute]ClusterRouteRequest paramRoute)
    {
        var data = await _clusterService.GetByIdAsync(paramRoute.Id);
        return Ok(data);
    } 

    [HttpPost()]
    [ProducesResponseType(typeof(CreateResponse<ClusterDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Post([FromBody]ClusterRequest request)
    {
        var response = await _clusterService.CreateAsync(request);
        return Created($"/cluster/{response.Id}", new CreateResponse<ClusterDto>(){
            Status = "OK",
            Message = "Cluster creado exitosamente.",
            Data = response
        });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateResponse<ClusterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute]ClusterRouteRequest paramsRoute, 
        [FromBody]ClusterRequest request)
    {
        var response = await _clusterService.UpdateAsync(paramsRoute.Id, request);
        return Ok(new UpdateResponse<ClusterDto>{
            Status = "OK",
            Message = "Cluster actualizado exitosamente.",
            Data = response
        });
    }

    [HttpPut("status/{id}")]
    [ProducesResponseType(typeof(UpdateResponse<ClusterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(
        [FromRoute]ClusterRouteRequest paramsRoute)
    {
        var response = await _clusterService.UpdateAsync(paramsRoute.Id);
        return Ok(new UpdateResponse<ClusterDto>(){
            Status = "OK",
            Message = "Cluster actualizado exitosamente.",
            Data = response
        });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DeleteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute]ClusterRouteRequest paramsRoute)
    {
        var response = await _clusterService.DeleteAsync(paramsRoute.Id);
        return Ok(response);
    }
}