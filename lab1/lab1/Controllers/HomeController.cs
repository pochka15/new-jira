using System;
using System.Collections.Generic;
using lab1.Models;
using lab1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Activity = System.Diagnostics.Activity;

namespace lab1.Controllers {
public class HomeController : Controller {
    private const string UserNameField = "UserName";
    private const string YearField = "Year";
    private const string MonthField = "Month";
    private const string DayField = "Day";
    private readonly IReportService _reportService;

    public HomeController(IReportService reportService) {
        _reportService = reportService;
    }

    private static DayReport BlankReport =>
        new() {
            Activities = new List<Models.Activity>(),
            Frozen = false
        };

    private SessionState SessionState {
        get {
            var userName = HttpContext.Session.GetString(UserNameField);
            var year = HttpContext.Session.GetString(YearField);
            var month = HttpContext.Session.GetString(MonthField);
            var day = HttpContext.Session.GetString(DayField);

            return new SessionState {
                UserName = userName,
                Year = year != null ? int.Parse(year) : null,
                Month = month != null ? int.Parse(month) : null,
                Day = day != null ? int.Parse(day) : null
            };
        }
    }

    public IActionResult Index() {
        var state = SessionState;
        state.Year ??= 2021;
        state.Month ??= 12;
        state.Day ??= 1;

        var origin = new ReportOrigin {
            Month = state.Month.Value,
            UserName = state.UserName,
            Year = state.Year.Value
        };
        var report = _reportService.GetDayReport(origin, state.Day.Value) ?? BlankReport;
        var date = DateTime.Parse(string.Join('/', state.Year.Value, state.Month.Value, state.Day.Value));
        var model = new ReportViewModel {
            Entries = report.Activities,
            Frozen = report.Frozen,
            Origin = origin,
            ChangeDateForm = new ChangeDateForm {Date = date},
            OverallTime = _reportService.CalcOverallTime(report.Activities),
            UserName = state.UserName
        };
        return View(model);
    }

    public IActionResult Privacy() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    public IActionResult ChangeDate(ChangeDateForm changeDateForm) {
        HttpContext.Session.SetString(YearField, changeDateForm.Date.Year.ToString());
        HttpContext.Session.SetString(MonthField, changeDateForm.Date.Month.ToString());
        HttpContext.Session.SetString(DayField, changeDateForm.Date.Day.ToString());
        return RedirectToAction("Index");
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
    public IActionResult ChangeUser(string userName) {
        HttpContext.Session.SetString(UserNameField, userName);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditActivity(
        int activityId,
        ReportOrigin reportOrigin,
        string project,
        string subCategory,
        int spentTime,
        string description) {
        var dto = new EditActivityDto {
            Id = activityId,
            Description = description,
            Project = project,
            SpentTime = spentTime,
            SubCategory = subCategory,
        };

        _reportService.EditEntry(reportOrigin, dto);
        return RedirectToAction("Index");
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
        var entry = report
            ?.Entries.Find(it => it.Id == activityId);
        if (entry == null) return View();
        var model = new EditActivityViewModel {
            ActivityId = activityId,
            ReportOrigin = reportOrigin,
            Description = entry.Description,
            Project = entry.ActivityCode,
            SpentTime = entry.Time,
            SubCategory = entry.SubCode
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteActivity(int activityId, ReportOrigin reportOrigin) {
        _reportService.DeleteEntryMatching(
            reportOrigin,
            entry => entry.Id == activityId
        );
        return RedirectToAction("Index");
    }
}

public class SessionState {
    public string UserName { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
}
}