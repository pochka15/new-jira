#nullable enable
using System.Collections.Generic;
using System.Linq;
using lab1.Dtos.Others;
using lab1.Dtos.Project;
using lab1.Dtos.Report;
using lab1.Models;

namespace lab1.Services {
public interface IReportService {
    DayActivities? GetDayReport(ReportOrigin origin, int day);
    MonthReportWithOrigin? GetMonthReport(ReportOrigin origin);
    IEnumerable<MonthReportWithOrigin> GetAllReports();
    MonthStatistics GetMonthStatistics(ReportOrigin origin);
    public MonthReportWithOrigin CreateBlankReport(ReportOrigin origin);
    void SubmitMonthActivities(ReportOrigin origin);
    IEnumerable<ReportOriginWithMeta> GetReportOriginsWithMeta(string projectId);

    static int SumTime(IEnumerable<ActivityDto> activities) {
        return (from activity in activities select activity.Time).Sum();
    }

    static int CalcOverallAcceptedTime(IEnumerable<ProjectCodeAndTime> timeSummaries) {
        return (from projectTime in timeSummaries select projectTime.Time).Sum();
    }
}
}