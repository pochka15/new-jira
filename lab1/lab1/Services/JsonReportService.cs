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
            Frozen = report.Frozen,
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

    public int CalcOverallTime(IEnumerable<Activity> activities) {
        return (from activity in activities select activity.Time).Sum();
    }

    public MonthStatistics GetMonthStatistics(ReportOrigin origin) {
        var report = GetMonthReport(origin);
        if (report == null) return new MonthStatistics();
        return new MonthStatistics {
            ProjectToTime = BuildProjectToTime(report),
            ProjectToAcceptedTime = BuildProjectToAcceptedTime(report)
        };
    }

    public MonthReport CreateBlankReport(ReportOrigin origin) {
        var report = GetMonthReport(origin);
        if (report != null) return report;

        var path = Path.Combine(_dataRoot, "activities", GetReportFileName(origin));
        report = new MonthReport {
            Activities = new List<Activity>(),
            Frozen = false,
            AcceptedActivities = new List<ProjectTime>()
        };
        File.WriteAllText(path, JsonSerializer.Serialize(report));
        return report;
    }

    public void LockMonth(ReportOrigin origin) {
        var report = GetMonthReport(origin)!;
        report.Frozen = true;
        var path = Path.Combine(_dataRoot, "activities", GetReportFileName(origin));
        File.WriteAllText(path, JsonSerializer.Serialize(report));
    }

    private static Dictionary<string, int> BuildProjectToAcceptedTime(MonthReport report) {
        var groups = from activity in report.AcceptedActivities
            group activity by activity.ProjectCode
            into projectGroup
            select projectGroup;
        return groups.ToDictionary(
            it => it.Key,
            gr => (from projectTime in gr select projectTime.Time).Sum());
    }

    private static Dictionary<string, int> BuildProjectToTime(MonthReport report) {
        var groups = from activity in report.Activities
            group activity by activity.ProjectCode
            into projectGroup
            select projectGroup;
        return groups.ToDictionary(pGroup => pGroup.Key, CalcOverallTime());
    }

    private static Func<IGrouping<string, Activity>, int> CalcOverallTime() {
        return gr => (from it in gr select it.Time).Sum();
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