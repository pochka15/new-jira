using lab1.Models;
using lab1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Activity = System.Diagnostics.Activity;

namespace lab1.Controllers {
public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly IActivitiesService _activitiesService;
    private readonly IReportService _reportService;

    public HomeController(
        ILogger<HomeController> logger,
        IActivitiesService activitiesService,
        IReportService reportService) {
        _logger = logger;
        _activitiesService = activitiesService;
        _reportService = reportService;
    }

    public IActionResult Index() {
        return View();
    }

    public IActionResult Privacy() {
        return View();
    }

    public IActionResult Test() {
        var model = new TestViewModel {
            Report = _reportService.GetUserReport("kowalski", 2021, 11)
        };
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}
}