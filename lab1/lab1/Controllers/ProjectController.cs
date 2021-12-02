#nullable enable
using System.Linq;
using lab1.Models;
using lab1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers {
public class ProjectController : Controller {
    private readonly IProjectService _projectService;
    private readonly IReportService _reportService;

    public ProjectController(IReportService reportService, IProjectService projectService) {
        _reportService = reportService;
        _projectService = projectService;
    }

    private SessionState SessionState {
        get {
            var userName = HttpContext.Session.GetString(SessionState.UserNameField);
            var year = HttpContext.Session.GetString(SessionState.YearField);
            var month = HttpContext.Session.GetString(SessionState.MonthField);
            var day = HttpContext.Session.GetString(SessionState.DayField);

            return new SessionState {
                UserName = userName,
                Year = year != null ? int.Parse(year) : null,
                Month = month != null ? int.Parse(month) : null,
                Day = day != null ? int.Parse(day) : null
            };
        }
    }

    [HttpGet]
    public IActionResult Index(string code) {
        var project = _projectService.GetProjectByCode(code)!;
        return View(new ProjectViewModel {
            Cost = project.Cost,
            ProjectName = project.Name,
            ProjectCode = code,
            LeftBudget = _projectService.CalcLeftBudget(project),
            InitialBudget = project.Budget,
            ProjectIsActive = project.IsActive,
            ReportOriginsWithMeta = _reportService.GetReportOriginsWithMeta(code)
                .OrderByDescending(it => it.Year)
                .ThenByDescending(it => it.Month)
        });
    }

    [HttpGet]
    public IActionResult CloseProject(string projectCode) {
        _projectService.CloseProject(projectCode);
        return RedirectToAction("Index", new {code = projectCode});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateCost(string projectCode, int cost) {
        _projectService.UpdateCost(projectCode, cost);
        return RedirectToAction("Index", new {code = projectCode});
    }

    public IActionResult MonthStatistics(int? year, int? month) {
        var state = SessionState;
        var y = year ?? state.Year!.Value;
        var m = month ?? state.Month!.Value;
        var statistics = _reportService.GetMonthStatistics(
            new ReportOrigin {Year = y, Month = m, UserName = state.UserName});

        var model = new MonthStatisticsViewModel {
            ProjectToTime = statistics.ProjectToTime,
            ProjectToAcceptedTime = statistics.ProjectToAcceptedTime,
            Year = y,
            Month = m,
            UserName = state.UserName
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult AcceptTime(int year, int month, string userName, string projectCode) {
        var model = new AcceptTimeViewModel {
            Origin = new ReportOrigin {
                UserName = userName,
                Year = year,
                Month = month
            },
            ProjectCode = projectCode
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult PostAcceptedTime(ReportOrigin origin, string projectCode, int acceptedTime) {
        _projectService.AcceptTime(origin, projectCode, acceptedTime);
        return RedirectToAction("Index", new {code = projectCode});
    }


    [HttpGet]
    public IActionResult Create() {
        return View();
    }

    // TODO(@pochka15): impr: do smth when there already exists some project
    [HttpPost]
    public IActionResult Create(string projectName, string code, int? budget, string? subprojectCodes) {
        var state = SessionState;
        _projectService.CreateProject(new CreateProjectDto {
            Budget = budget ?? 0,
            Code = code,
            Manager = state.UserName,
            ProjectName = projectName,
            SubprojectCodes = subprojectCodes ?? ""
        });
        return RedirectToAction("Index", "Menu");
    }
}
}