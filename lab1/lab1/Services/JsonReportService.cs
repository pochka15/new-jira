#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using lab1.Dtos.Others;
using lab1.Dtos.Project;
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
            where Path.GetFileNameWithoutExtension(path)
                .Equals(origin.UserName + "-" + origin.Year + "-" + origin.Month)
            select DeserializeReport(path, origin);
        var report = query.FirstOrDefault();

        if (report == null) return null;
        report.Activities.RemoveAll(it => DateTime.Parse(it.Date).Day != day);

        return new DayActivities {
            Frozen = report.IsFrozen,
            Activities = report.Activities.Select(it => it.ToActivityDto()).ToList()
        };
    }

    public MonthReportWithOrigin? GetMonthReport(ReportOrigin origin) {
        var root = Path.Combine(_dataRoot, "activities");
        var files = Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories);
        var query = from path in files
            where Path.GetFileName(path).Equals(GetReportFileName(origin))
            select DeserializeReport(path, origin);
        return query.Select(it => it.ToMonthReportWithOrigin()).FirstOrDefault();
    }

    public IEnumerable<MonthReportWithOrigin> GetAllReports() {
        var root = Path.Combine(_dataRoot, "activities");
        var files = Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories);
        var reports = from path in files
            select DeserializeReport(path, ParseReportOrigin(path));
        return reports.Select(it => it.ToMonthReportWithOrigin());
    }

    public MonthStatistics GetMonthStatistics(ReportOrigin origin) {
        var report = GetMonthReport(origin);
        if (report == null) return new MonthStatistics();
        return new MonthStatistics {
            ProjectToTime = BuildProjectToTime(report),
            ProjectToAcceptedTime = GetProjectToAcceptedTime(report)
        };
    }

    public MonthReportWithOrigin CreateBlankReport(ReportOrigin origin) {
        var report = GetMonthReport(origin);
        if (report != null) return report;

        var path = Path.Combine(_dataRoot, "activities", GetReportFileName(origin));
        report = new MonthReportWithOrigin {
            Activities = new List<ActivityDto>(),
            IsFrozen = false,
            AcceptedWork = new List<ProjectCodeAndTime>(),
            Origin = origin
        };
        Store(path, report.ToModel());
        return report;
    }

    public void SubmitMonthActivities(ReportOrigin origin) {
        var report = GetMonthReport(origin)!;
        report.IsFrozen = true;
        var path = Path.Combine(_dataRoot, "activities", GetReportFileName(origin));
        Store(path, report.ToModel());
    }

    public IEnumerable<ReportOriginWithMeta> GetReportOriginsWithMeta(string projectId) {
        return GetAllReports()
            .Select(it => {
                it.Activities = it.Activities
                    .Where(activity => activity.ProjectCode == projectId)
                    .ToList();
                return it;
            })
            .Where(it => it.Activities.Any())
            .Select(it => it.ToReportOriginWithMeta(projectId));
    }

    private static void Store(string path, MonthReport report) {
        File.WriteAllText(path, JsonSerializer.Serialize(report));
    }

    private static ReportOrigin ParseReportOrigin(string path) {
        var parts = Path.GetFileNameWithoutExtension(path).Split("-");
        return new ReportOrigin {
            UserName = parts[0],
            Year = Convert.ToInt32(parts[1]),
            Month = Convert.ToInt32(parts[2])
        };
    }

    public static Dictionary<string, int> GetProjectToAcceptedTime(MonthReportWithoutOrigin reportWithoutOrigin) {
        var groups = from activity in reportWithoutOrigin.AcceptedWork
            group activity by activity.Id
            into projectGroup
            select projectGroup;
        return groups.ToDictionary(
            it => it.Key,
            it => IReportService.CalcOverallAcceptedTime(it.AsEnumerable()));
    }

    public static Dictionary<string, int> BuildProjectToTime(MonthReportWithoutOrigin reportWithoutOrigin) {
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

    private static MonthReport DeserializeReport(string path, ReportOrigin origin) {
        using var reader = File.OpenText(path);
        var report = JsonSerializer.Deserialize<MonthReport>(reader.ReadToEnd(),
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            })!;
        report.Year = origin.Year;
        report.Month = origin.Month;
        report.UserName = origin.UserName;
        return report;
    }

    public static ProjectCodeAndTime ExtractSummary(string projectCode, MonthReportWithoutOrigin reportWithoutOrigin) {
        return reportWithoutOrigin.AcceptedWork
                   .FirstOrDefault(s => s.Id == projectCode)
               ?? new ProjectCodeAndTime(projectCode);
    }
}
}