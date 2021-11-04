#nullable enable
using System;
using System.Collections.Generic;
using lab1.Models;

namespace lab1.Services {
public interface IReportService {
    DayReport? GetDayReport(ReportOrigin origin, int day);
    MonthReport? GetMonthReport(ReportOrigin origin);
    MonthReport DeleteEntryMatching(ReportOrigin reportOrigin, Predicate<ReportEntry> pred);
    MonthReport EditEntry(ReportOrigin reportOrigin, EditEntryDto dto);
    int CalcOverallTime(IEnumerable<ReportEntry> reports);
    MonthStatistics GetMonthStatistics(ReportOrigin origin);
}
}