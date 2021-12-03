#nullable enable
using System.Diagnostics;
using System.Linq;
using lab1.Dtos.Others;
using lab1.Dtos.Report;
using lab1.Extensions;
using lab1.Models;
using lab1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers {
public class ActivityController : Controller {
    private readonly IProjectService _projectService;
    private readonly IReportService _reportService;

    public ActivityController(IReportService reportService, IProjectService projectService) {
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
    public IActionResult Add() {
        var model = new AddActivityViewModel {
            Projects = _projectService.GetActiveProjects()
                .Select(it => it.ToSelectItem())
                .ToList()
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Delete(int activityId, ReportOrigin reportOrigin) {
        _projectService.DeleteActivityMatching(
            reportOrigin,
            it => it.Id == activityId
        );
        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Edit activity form
    /// </summary>
    /// <param name="activityId">activity id</param>
    /// <param name="reportOrigin">report origin</param>
    /// <returns>A view containing the edit activity form</returns>
    [HttpGet]
    public IActionResult Edit(int activityId, ReportOrigin reportOrigin) {
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
            SubprojectCode = activity.SubprojectCode,
            Projects = _projectService.GetActiveProjects()
                .Select(it => it.ToSelectItem())
                .ToList()
        };
        return View(model);
    }


    [HttpPost]
    public IActionResult Add(
        string projectCode,
        string? subprojectCode,
        int spentTime,
        string? description) {
        var state = SessionState;
#pragma warning disable 8629
        Debug.Assert(!state.HasNullFields);
        var origin = new ReportOrigin {
            UserName = state.UserName,
            Year = state.Year.Value,
            Month = state.Month.Value
        };
        var addActivityDto = new AddActivityDto {
            ProjectCode = projectCode,
            SubprojectCode = subprojectCode ?? "",
            SpentTime = spentTime,
            Description = description ?? "",
            Day = state.Day.Value
        };
        _projectService.AddActivity(origin, addActivityDto);
#pragma warning restore 8629
        return RedirectToAction("Index", "Home");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(
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

        _projectService.EditActivity(reportOrigin, dto);
        return RedirectToAction("Index", "Home");
    }
}
}