using Microsoft.AspNetCore.Mvc;

namespace kafkaclient.web.Core.Dto;

public class PagingRequest
{
    [FromQuery(Name = "offset")]
    public int? Offset { get; set; }
    [FromQuery(Name = "limit")]
    public int? Limit { get; set; }
}