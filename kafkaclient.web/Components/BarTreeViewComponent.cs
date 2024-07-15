using kafkaclient.web.Core.Dto;
using kafkaclient.web.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace kafkaclient.web.Components;

public class BarTreeViewComponent : ViewComponent
{
    private readonly ClusterService _clusterService;
    
    public BarTreeViewComponent(ClusterService clusterService)
    {
        _clusterService = clusterService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var query_paging = new PagingRequest { Offset = 1, Limit = 100 };
        var query_filter = new ClusterFilterRequest { Status = true };
        var query_order_by = new ClusterOrderByRequest {};
        var data = await _clusterService.GetAllAsync(query_paging, query_filter, query_order_by);
        
        return View(data);
    }
}