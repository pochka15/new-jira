#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using lab1.Dtos.Others;
using lab1.Dtos.Report;
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

    public DayActivities? GetDayReport(ReportOrigin origin, int day) {
        var root = Path.Combine(_dataRoot, "activities");
        var files = Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories);
        var query = from path in files
            where Path.GetFileName(path).Equals(origin.UserName + "-" + origin.Year + "-" + origin.Month + ".json")
            select DeserializeReport(path);
        var report = query.FirstOrDefault();

        if (report == null) return null;
        report.Activities.RemoveAll(it => DateTime.Parse(it.Date).Day != day);

        return new DayActivities {
            Frozen = report.IsFrozen,
            Activities = report.Activities
        };
    }

    public MonthReportWithoutOrigin? GetMonthReport(ReportOrigin origin) {
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
        return from path in files select DeserializeReportWithOrigin(path);
    }

    public MonthStatistics GetMonthStatistics(ReportOrigin origin) {
        var report = GetMonthReport(origin);
        if (report == null) return new MonthStatistics();
        return new MonthStatistics {
            ProjectToTime = BuildProjectToTime(report),
            ProjectToAcceptedTime = GetProjectToAcceptedTime(report)
        };
    }

    public MonthReportWithoutOrigin CreateBlankReport(ReportOrigin origin) {
        var report = GetMonthReport(origin);
        if (report != null) return report;

        var path = Path.Combine(_dataRoot, "activities", GetReportFileName(origin));
        report = new MonthReportWithoutOrigin {
            Activities = new List<Activity>(),
            IsFrozen = false,
            AcceptedWork = new List<ProjectCodeAndTime>()
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

    public IEnumerable<ReportOriginWithMeta> GetReportOriginsWithMeta(string projectCode) {
        return GetAllReports()
            .Select(it => {
                it.Activities = it.Activities
                    .Where(activity => activity.ProjectCode == projectCode)
                    .ToList();
                return it;
            })
            .Where(it => it.Activities.Any())
            .Select(it => new ReportOriginWithMeta {
                UserName = it.Origin.UserName,
                Year = it.Origin.Year,
                Month = it.Origin.Month,
                Time = IReportService.SumTime(it.Activities),
                AcceptedTime = ExtractSummary(projectCode, it).Time,
                IsFrozen = it.IsFrozen
            });
    }

    private static DateTime ToDate(int year, int month) {
        return new DateTime(year, month, 1);
    }

    private static ReportOrigin ParseReportOrigin(string path) {
        var parts = Path.GetFileNameWithoutExtension(path).Split("-");
        return new ReportOrigin {
            UserName = parts[0],
            Year = Convert.ToInt32(parts[1]),
            Month = Convert.ToInt32(parts[2])
        };
    }

    private static Dictionary<string, int> GetProjectToAcceptedTime(MonthReportWithoutOrigin reportWithoutOrigin) {
        var groups = from activity in reportWithoutOrigin.AcceptedWork
            group activity by activity.Id
            into projectGroup
            select projectGroup;
        return groups.ToDictionary(
            it => it.Key,
            it => IReportService.CalcOverallAcceptedTime(it.AsEnumerable()));
    }

    private static Dictionary<string, int> BuildProjectToTime(MonthReportWithoutOrigin reportWithoutOrigin) {
        var groups = from activity in reportWithoutOrigin.Activities
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

    private static MonthReportWithoutOrigin DeserializeReport(string path) {
        using var reader = File.OpenText(path);
        return JsonSerializer.Deserialize<MonthReportWithoutOrigin>(reader.ReadToEnd(),
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            })!;
    }

    private static MonthReportWithOrigin DeserializeReportWithOrigin(string path) {
        using var reader = File.OpenText(path);
        var report = JsonSerializer.Deserialize<MonthReportWithOrigin>(
            reader.ReadToEnd(),
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            })!;
        report.Origin = ParseReportOrigin(path);
        return report;
    }

    private static ProjectCodeAndTime ExtractSummary(string projectCode, MonthReportWithoutOrigin reportWithoutOrigin) {
        return reportWithoutOrigin.AcceptedWork
                   .FirstOrDefault(s => s.Id == projectCode)
               ?? new ProjectCodeAndTime(projectCode);
    }
}
}