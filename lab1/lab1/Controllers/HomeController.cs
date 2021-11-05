#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using lab1.Models;
using lab1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Activity = System.Diagnostics.Activity;

namespace lab1.Controllers {
public class HomeController : Controller {
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

    public IActionResult LockMonth() {
        var state = SessionState;
        _reportService.LockMonth(new ReportOrigin {
            UserName = state.UserName,
            Year = state.Year!.Value,
            Month = state.Month!.Value
        });
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Index() {
        var state = SessionState;
        // default magic constants
        if (state.Year == null) state = UpdateState(2021, 12, 7);

        Debug.Assert(state.Year != null && state.Month != null && state.Day != null,
            "state.Year != null && state.Month != null && state.Day != null");

        var origin = new ReportOrigin {
            Month = state.Month.Value,
            UserName = state.UserName,
            Year = state.Year.Value
        };
        var report = _reportService.GetDayReport(origin, state.Day.Value) ?? BlankReport;
        var date = DateTime.Parse(string.Join('/', state.Year.Value, state.Month.Value, state.Day.Value));
        var model = new ReportViewModel {
            Activities = report.Activities,
            Frozen = report.Frozen,
            Origin = origin,
            ChangeDateForm = new ChangeDateForm {Date = date},
            OverallTime = _reportService.CalcOverallTime(report.Activities),
            UserName = state.UserName
        };
        return View(model);
    }

    private SessionState UpdateState(int year, int month, int day) {
        HttpContext.Session.SetString(SessionState.YearField, year.ToString());
        HttpContext.Session.SetString(SessionState.MonthField, month.ToString());
        HttpContext.Session.SetString(SessionState.DayField, day.ToString());
        return new SessionState {
            UserName = HttpContext.Session.GetString(SessionState.UserNameField),
            Year = year,
            Month = month,
            Day = day
        };
    }

    public IActionResult Privacy() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    public IActionResult ChangeDate(ChangeDateForm changeDateForm) {
        UpdateState(changeDateForm.Date.Year, changeDateForm.Date.Month, changeDateForm.Date.Day);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult ChangeUser(string userName) {
        HttpContext.Session.SetString(SessionState.UserNameField, userName);
        return RedirectToAction("Index");
    }
}
}