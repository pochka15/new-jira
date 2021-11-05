#nullable enable
using lab1.Models;
using lab1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers {
public class ProjectController : Controller {
    private readonly IProjectsService _projectsService;
    private readonly IReportService _reportService;

    public ProjectController(IReportService reportService, IProjectsService projectsService) {
        _reportService = reportService;
        _projectsService = projectsService;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditActivity(
        int activityId,
        ReportOrigin reportOrigin,
        string projectCode,
        string subprojectCode,
        int spentTime,
        string description) {
        var dto = new EditActivityDto {
            Id = activityId,
            Description = description,
            ProjectCode = projectCode,
            SpentTime = spentTime,
            SubprojectCode = subprojectCode,
        };

        _reportService.EditActivity(reportOrigin, dto);
        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Edit activity form
    /// </summary>
    /// <param name="activityId">activity id</param>
    /// <param name="reportOrigin">report origin</param>
    /// <returns>A view containing the edit activity form</returns>
    [HttpPost]
    public IActionResult EditActivityForm(int activityId, ReportOrigin reportOrigin) {
        var report = _reportService.GetMonthReport(reportOrigin);
        var activity = report
            ?.Activities.Find(it => it.Id == activityId);
        if (activity == null) return View();
        var model = new EditActivityViewModel {
            ActivityId = activityId,
            ReportOrigin = reportOrigin,
            Description = activity.Description,
            ProjectCode = activity.ProjectCode,
            SpentTime = activity.Time,
            SubprojectCode = activity.SubprojectCode
        };
        return View(model);
    }

    public IActionResult MonthStatistics(int? year, int? month) {
        var state = SessionState;
        var y = year ?? state.Year!.Value;
        var m = month ?? state.Month!.Value;
        var statistics = _reportService.GetMonthStatistics(
            new ReportOrigin {Year = y, Month = m, UserName = state.UserName});

        var model = new MonthStatisticsViewModel {
            ProjectToTime = statistics.ProjectToTime,
            Year = y,
            Month = m,
            UserName = state.UserName
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteActivity(int activityId, ReportOrigin reportOrigin) {
        _reportService.DeleteActivityMatching(
            reportOrigin,
            it => it.Id == activityId
        );
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult CreateProject() {
        return View();
    }

    // TODO(@pochka15): impr: do smth when there already exists some project
    [HttpPost]
    public IActionResult CreateProject(string projectName, string code, int? budget, string? subprojectCodes) {
        var state = SessionState;
        _projectsService.CreateProject(new CreateProjectDto {
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