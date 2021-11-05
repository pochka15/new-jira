#nullable enable
using System;
using System.Collections.Generic;
using lab1.Models;

namespace lab1.Services {
public interface IReportService {
    DayReport? GetDayReport(ReportOrigin origin, int day);
    MonthReport? GetMonthReport(ReportOrigin origin);
    MonthReport DeleteActivityMatching(ReportOrigin reportOrigin, Predicate<Activity> pred);
    MonthReport EditActivity(ReportOrigin reportOrigin, EditActivityDto dto);
    int CalcOverallTime(IEnumerable<Activity> reports);

    MonthStatistics GetMonthStatistics(ReportOrigin origin);
    // TODO(@pochka15): add activity
    // it stores new activity -> some report
}
}