using Microsoft.AspNetCore.Mvc;

namespace kafkaclient.web.Core.Dto;

public class ClusterOrderByRequest
{
    [FromQuery(Name="order_by_id")]
    public string? OrderById { get; set; }
    [FromQuery(Name="order_by_name")]
    public string? OrderByName { get; set; }
    [FromQuery(Name="order_by_version")]
    public string? OrderByVersion { get; set; }
    [FromQuery(Name="order_by_host")]
    public string? OrderByHost { get; set; }
    [FromQuery(Name="order_by_path")]
    public string? OrderByPath { get; set; }

    [FromQuery(Name="order_by_status")]
    public string? OrderByStatus { get; set; }
}