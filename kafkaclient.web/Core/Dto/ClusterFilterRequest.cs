using Microsoft.AspNetCore.Mvc;

namespace kafkaclient.web.Core.Dto;

public class ClusterFilterRequest
{
    [FromQuery(Name="search")]
    public string? Search { get; set; }

    [FromQuery(Name="status")]
    public bool? Status { get; set; } = null;
}