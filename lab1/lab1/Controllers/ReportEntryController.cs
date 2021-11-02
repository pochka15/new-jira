using lab1.Models;
using lab1.Services;
using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers {
public class ReportEntryController : Controller {
    private readonly IReportService _reportService;

    public ReportEntryController(IReportService reportService) {
        _reportService = reportService;
    }

    [HttpPost]
    public IActionResult Index(EntryDescription entryDescription) {
        var report = _reportService.GetMonthReport(entryDescription.ReportOrigin);
        var entry = report
            ?.Entries.Find(it => it.Id == entryDescription.EntryId);
        if (entry == null) return View();
        var model = new EditEntryViewModel {
            EntryDescription = entryDescription,
            Description = entry.Description,
            Project = entry.ActivityCode,
            SpentTime = entry.Time,
            SubCategory = entry.SubCode
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(EntryDescription entryDescription) {
        _reportService.DeleteEntryMatching(
            entryDescription.ReportOrigin,
            entry => entry.Id == entryDescription.EntryId
        );
        return RedirectToAction("Index", "Report");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(
        EntryDescription entryDescription,
        string project,
        string subCategory,
        int spentTime,
        string description) {
        var dto = new EditEntryDto {
            Id = entryDescription.EntryId,
            Description = description,
            Project = project,
            SpentTime = spentTime,
            SubCategory = subCategory,
        };

        _reportService.EditEntry(entryDescription.ReportOrigin, dto);
        return RedirectToAction("Index", "Report");
    }
}
}