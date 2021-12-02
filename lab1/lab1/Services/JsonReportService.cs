#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using lab1.Models;

namespace lab1.Services {
public class JsonReportService : IReportService {
    private readonly string _dataRoot;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="src">source of the path to the data</param>
    public JsonReportService(DataPathSrc src) {
        _dataRoot = src.Path;
    }

    public DayReport? GetDayReport(ReportOrigin origin, int day) {
        var root = Path.Combine(_dataRoot, "activities");
        var files = Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories);
        var query = from path in files
            where Path.GetFileName(path).Equals(origin.UserName + "-" + origin.Year + "-" + origin.Month + ".json")
            select DeserializeReport(path);
        var report = query.FirstOrDefault();

        if (report == null) return null;
        report.Activities.RemoveAll(it => DateTime.Parse(it.Date).Day != day);

        return new DayReport {
            Frozen = report.IsFrozen,
            Activities = report.Activities
        };
    }

    public MonthReport? GetMonthReport(ReportOrigin origin) {
        var root = Path.Combine(_dataRoot, "activities");
        var files = Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories);
        var query = from path in files
            where Path.GetFileName(path).Equals(GetReportFileName(origin))
            select DeserializeReport(path);
        return query.FirstOrDefault();
    }

    public IEnumerable<MonthReportWithOrigin> GetAllReports() {
        var root = Path.Combine(_dataRoot, "activities");
        var files = Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories);
        return from path in files select DeserializeReport(path);
    }

    public MonthStatistics GetMonthStatistics(ReportOrigin origin) {
        var report = GetMonthReport(origin);
        if (report == null) return new MonthStatistics();
        return new MonthStatistics {
            ProjectToTime = BuildProjectToTime(report),
            ProjectToAcceptedTime = GetProjectToAcceptedTime(report)
        };
    }

    public MonthReport CreateBlankReport(ReportOrigin origin) {
        var report = GetMonthReport(origin);
        if (report != null) return report;

        var path = Path.Combine(_dataRoot, "activities", GetReportFileName(origin));
        report = new MonthReport {
            Activities = new List<Activity>(),
            IsFrozen = false,
            TimeSummaries = new List<ProjectTimeSummary>()
        };
        File.WriteAllText(path, JsonSerializer.Serialize(report));
        return report;
    }

    public void SubmitMonthActivities(ReportOrigin origin) {
        var report = GetMonthReport(origin)!;
        report.IsFrozen = true;
        var path = Path.Combine(_dataRoot, "activities", GetReportFileName(origin));
        File.WriteAllText(path, JsonSerializer.Serialize(report));
    }

    private static Dictionary<string, int> GetProjectToAcceptedTime(MonthReport report) {
        var groups = from activity in report.TimeSummaries
            group activity by activity.ProjectCode
            into projectGroup
            select projectGroup;
        return groups.ToDictionary(
            it => it.Key,
            it => IReportService.CalcOverallAcceptedTime(it.AsEnumerable()));
    }

    private static Dictionary<string, int> BuildProjectToTime(MonthReport report) {
        var groups = from activity in report.Activities
            group activity by activity.ProjectCode
            into projectGroup
            select projectGroup;
        return groups.ToDictionary(
            it => it.Key,
            it => IReportService.SumTime(it.AsEnumerable()));
    }

    public static string GetReportFileName(ReportOrigin origin) {
        return origin.UserName + "-" + origin.Year + "-" + origin.Month + ".json";
    }

    private static MonthReport DeserializeReport(string path) {
        using var reader = File.OpenText(path);
        return JsonSerializer.Deserialize<MonthReport>(reader.ReadToEnd(),
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            })!;
    }
}
}