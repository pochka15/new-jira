#nullable enable
using System.Collections.Generic;
using System.Linq;
using lab1.Models;

namespace lab1.Services {
public interface IReportService {
    DayReport? GetDayReport(ReportOrigin origin, int day);
    MonthReport? GetMonthReport(ReportOrigin origin);
    IEnumerable<MonthReportWithOrigin> GetAllReports();
    MonthStatistics GetMonthStatistics(ReportOrigin origin);
    public MonthReport CreateBlankReport(ReportOrigin origin);
    void SubmitMonthActivities(ReportOrigin origin);
    IEnumerable<ReportOriginWithMeta> GetReportOriginsWithMeta(string projectCode);

    static int SumTime(IEnumerable<Activity> activities) {
        return (from activity in activities select activity.Time).Sum();
    }

    static int CalcOverallAcceptedTime(IEnumerable<ProjectTimeSummary> timeSummaries) {
        return (from projectTime in timeSummaries select projectTime.Time).Sum();
    }
}
}