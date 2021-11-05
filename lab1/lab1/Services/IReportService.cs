#nullable enable
using System.Collections.Generic;
using System.Linq;
using lab1.Models;

namespace lab1.Services {
public interface IReportService {
    DayReport? GetDayReport(ReportOrigin origin, int day);
    MonthReport? GetMonthReport(ReportOrigin origin);
    IEnumerable<MonthReport> GetAllReports();
    MonthStatistics GetMonthStatistics(ReportOrigin origin);
    public MonthReport CreateBlankReport(ReportOrigin origin);
    void LockMonth(ReportOrigin origin);

    static int CalcOverallTime(IEnumerable<Activity> activities) {
        return (from activity in activities select activity.Time).Sum();
    }

    static int CalcOverallAcceptedTime(IEnumerable<ProjectTime> projectTimes) {
        return (from projectTime in projectTimes select projectTime.Time).Sum();
    }
}
}