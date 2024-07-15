using Microsoft.AspNetCore.Mvc;

namespace kafkaclient.web.Core.Dto;

public class ClusterRouteRequest
{
    [FromRoute(Name = "id")]
    public int Id { get; set; }
}