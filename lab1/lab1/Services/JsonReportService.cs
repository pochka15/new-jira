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
    /// <param name="dataRoot">a root folder containing the json data</param>
    public JsonReportService(string dataRoot) {
        _dataRoot = dataRoot;
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

    public MonthReport DeleteActivityMatching(ReportOrigin origin, Predicate<Activity> pred) {
        var report = GetMonthReport(origin)!;
        var path = Path.Combine(_dataRoot, "activities", GetReportFileName(origin));

        report.Activities.RemoveAll(pred);
        File.WriteAllText(path, JsonSerializer.Serialize(report));

        return report;
    }

    public MonthReport EditActivity(ReportOrigin origin, EditActivityDto dto) {
        var report = GetMonthReport(origin)!;
        var path = Path.Combine(_dataRoot, "activities", GetReportFileName(origin));

        foreach (var activity in report.Activities.Where(it => it.Id == dto.Id)) {
            CopyDataFromDto(activity, dto);
            break;
        }

        File.WriteAllText(path, JsonSerializer.Serialize(report));

        return report;
    }

    public int CalcOverallTime(IEnumerable<Activity> reports) {
        return (from activity in reports select activity.Time).Sum();
    }

    public MonthStatistics GetMonthStatistics(ReportOrigin origin) {
        var report = GetMonthReport(origin);
        if (report == null) return new MonthStatistics {ProjectToTime = new Dictionary<string, int>()};

        var query = from activity in report.Activities
            group activity by activity.ActivityCode
            into projectGroup
            select projectGroup;
        var projectToTime = query.ToDictionary(pGroup => pGroup.Key, CalcOverallTime());
        return new MonthStatistics {ProjectToTime = projectToTime};
    }

    private static Func<IGrouping<string, Activity>, int> CalcOverallTime() {
        return pGroup
            => (from it in pGroup select it.Time).Sum();
    }

    private static string GetReportFileName(ReportOrigin origin) {
        return origin.UserName
               + "-" + origin.Year + "-"
               + origin.Month + ".json";
    }

    private static void CopyDataFromDto(Activity activity, EditActivityDto dto) {
        activity.ActivityCode = dto.Project;
        activity.Description = dto.Description;
        activity.Time = dto.SpentTime;
        activity.SubCode = dto.SubCategory;
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