#nullable enable
using System;
using lab1.Models;
using lab1.Services;
using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers {
public class ReportController : Controller {
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService) {
        _reportService = reportService;
    }

    public IActionResult Index(string? userName, int? year, int? month, int? day) {
        userName ??= "kowalski";
        year ??= 2021;
        month ??= 12;
        day ??= 1;

        var origin = new ReportOrigin {Month = month, UserName = userName, Year = year};
        var report = _reportService.GetDayReport(origin, day.Value);
        var date = DateTime.Parse(string.Join('/', year.Value, month.Value, day.Value));
        var model = new ReportViewModel {
            Entries = report!.Entries,
            Frozen = report.Frozen,
            Origin = origin,
            ChangeDateForm = new ChangeDateForm {Date = date, UserName = userName},
            OverallTime = _reportService.CalcOverallTime(report.Entries)
        };
        return View(model);
    }

    public IActionResult ChangeDate(ChangeDateForm changeDateForm) {
        var origin = new ReportOrigin {
            Month = changeDateForm.Date.Month,
            Year = changeDateForm.Date.Year,
            UserName = changeDateForm.UserName
        };
        return RedirectToAction("Index", new {
            origin.Month, origin.Year, origin.UserName, changeDateForm.Date.Day
        });
    }
}
}