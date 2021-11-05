#nullable enable
using System;
using lab1.Models;
using lab1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers {
public class ReportController : Controller {
    private const string UserNameField = "UserName";
    private const string YearField = "Year";
    private const string MonthField = "Month";
    private const string DayField = "Day";
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService) {
        _reportService = reportService;
    }

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
        state.UserName ??= "kowalski";
        state.Year ??= 2021;
        state.Month ??= 12;
        state.Day ??= 1;

        var origin = new ReportOrigin {Month = state.Month, UserName = state.UserName, Year = state.Year};
        var report = _reportService.GetDayReport(origin, state.Day.Value);
        var date = DateTime.Parse(string.Join('/', state.Year.Value, state.Month.Value, state.Day.Value));
        var model = new ReportViewModel {
            Entries = report!.Entries,
            Frozen = report.Frozen,
            Origin = origin,
            ChangeDateForm = new ChangeDateForm {Date = date, UserName = state.UserName},
            OverallTime = _reportService.CalcOverallTime(report.Entries),
            UserName = state.UserName
        };
        return View(model);
    }

    public IActionResult ChangeDate(ChangeDateForm changeDateForm) {
        HttpContext.Session.SetString(YearField, changeDateForm.Date.Year.ToString());
        HttpContext.Session.SetString(MonthField, changeDateForm.Date.Month.ToString());
        HttpContext.Session.SetString(DayField, changeDateForm.Date.Day.ToString());
        return RedirectToAction("Index");
    }

    public IActionResult MonthStatistics(string? userName, int? year, int? month) {
        userName ??= "kowalski";
        year ??= 2021;
        month ??= 12;

        var statistics = _reportService.GetMonthStatistics(
            new ReportOrigin {Year = year, Month = month, UserName = userName});
        var model = new MonthStatisticsViewModel {
            ProjectToTime = statistics.ProjectToTime,
            Year = year.Value,
            Month = month.Value,
            UserName = userName
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult ChangeUser(string userName) {
        HttpContext.Session.SetString(UserNameField, userName);
        return RedirectToAction("Index");
    }
}

public class SessionState {
    public string? UserName { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
}
}