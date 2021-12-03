using System.Linq;
using lab1.Models;
using lab1.Services;

namespace lab1.Dtos.Report {
public class MonthReportWithOrigin : MonthReportWithoutOrigin {
    public ReportOrigin Origin { get; set; }

    public MonthReport ToModel() {
        return new MonthReport {
            UserName = Origin.UserName,
            Year = Origin.Year,
            Month = Origin.Month,
            IsFrozen = IsFrozen,
            Activities = Activities.Select(it => it.ToModel()).ToList(),
            AcceptedWork = AcceptedWork
        };
    }

    public ReportOriginWithMeta ToReportOriginWithMeta(string projectId) {
        return new ReportOriginWithMeta {
            UserName = Origin.UserName,
            Year = Origin.Year,
            Month = Origin.Month,
            Time = IReportService.SumTime(Activities),
            AcceptedTime = JsonReportService.ExtractSummary(projectId, this).Time,
            IsFrozen = IsFrozen
        };
    }
}
}