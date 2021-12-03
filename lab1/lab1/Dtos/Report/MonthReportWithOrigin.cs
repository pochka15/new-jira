using System.Text.Json.Serialization;
using lab1.Models;

namespace lab1.Dtos.Report {
public class MonthReportWithOrigin : MonthReportWithoutOrigin {
    [JsonIgnore] public ReportOrigin Origin { get; set; }

    public MonthReport ToMonthReport() {
        return new MonthReport {
            UserName = Origin.UserName,
            Year = Origin.Year,
            Month = Origin.Month,
            IsFrozen = IsFrozen,
            Activities = Activities,
            AcceptedWork = AcceptedWork
        };
    }
}
}