using kafkaclient.web.Models;
using Microsoft.AspNetCore.Mvc;

namespace kafkaclient.web.Controllers;

public class ClusterController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public ClusterController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
}