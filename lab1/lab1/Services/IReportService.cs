#nullable enable
using System.Collections.Generic;
using lab1.Models;

namespace lab1.Services {
public interface IReportService {
    DayReport? GetDayReport(ReportOrigin origin, int day);
    MonthReport? GetMonthReport(ReportOrigin origin);
    int CalcOverallTime(IEnumerable<Activity> activities);
    MonthStatistics GetMonthStatistics(ReportOrigin origin);
    public MonthReport CreateBlankReport(ReportOrigin origin);
}
}