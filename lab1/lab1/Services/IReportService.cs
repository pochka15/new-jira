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
    MonthReport AddActivity(ReportOrigin origin, AddActivityDto dto);
    int CalcOverallTime(IEnumerable<Activity> activities);
    MonthStatistics GetMonthStatistics(ReportOrigin origin);
}
}