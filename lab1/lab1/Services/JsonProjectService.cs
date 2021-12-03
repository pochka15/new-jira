#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using lab1.Models;
using Activity = lab1.Models.Activity;

namespace lab1.Services {
public class JsonProjectService : IProjectService {
    private readonly string _dataRoot;
    private readonly IReportService _reportService;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="src">source of the path to the data</param>
    /// <param name="reportService">report service</param>
    public JsonProjectService(DataPathSrc src, IReportService reportService) {
        _dataRoot = src.Path;
        _reportService = reportService;
    }

    public void EditActivity(ReportOrigin origin, EditActivityDto dto) {
        var isActive = GetProjectById(dto.ProjectCode)!.IsActive;
        Debug.Assert(isActive);

        var report = _reportService.GetMonthReport(origin)!;
        foreach (var activity in report.Activities.Where(it => it.Id == dto.Id)) {
            CopyDataFromDto(activity, dto);
            break;
        }

        Store(report, origin);
    }

    public void DeleteActivityMatching(ReportOrigin origin, Predicate<Activity> pred) {
        var report = _reportService.GetMonthReport(origin)!;
        var path = Path.Combine(_dataRoot, "activities", JsonReportService.GetReportFileName(origin));

        report.Activities.RemoveAll(pred);
        File.WriteAllText(path, JsonSerializer.Serialize(report));
    }

    public void AddActivity(ReportOrigin origin, AddActivityDto dto) {
        var isActive = GetProjectById(dto.ProjectCode)!.IsActive;
        Debug.Assert(isActive);

        var report = _reportService.GetMonthReport(origin) ?? _reportService.CreateBlankReport(origin);
        var path = Path.Combine(_dataRoot, "activities", JsonReportService.GetReportFileName(origin));
        var id = GetNextId(report.Activities);

        report.Activities.Add(new Activity {
            Id = id,
            Date = string.Join('/', origin.Year, origin.Month, dto.Day),
            ProjectCode = dto.ProjectCode,
            SubprojectCode = dto.SubprojectCode,
            Time = dto.SpentTime,
            Description = dto.Description
        });
        File.WriteAllText(path, JsonSerializer.Serialize(report));
    }

    public void UpdateCost(string projectId, int cost) {
        var projects = GetAllProjects().ToList();
        var project = projects.FirstOrDefault(it => it.Id == projectId);
        if (project != null) project.Cost = cost;

        File.WriteAllText(Path.Combine(_dataRoot, "activities.json"),
            JsonSerializer.Serialize(new ProjectsContainer {Projects = projects}));
    }

    public int CalcLeftBudget(Project project) {
        return project.Budget - CalcOverallAcceptedTime(project.Id) * project.Cost;
    }

    public void AcceptTime(ReportOrigin origin, string projectId, int time) {
        var report = _reportService.GetMonthReport(origin)!;
        var summary = report.AcceptedWork
            .FirstOrDefault(it => it.Id == projectId);
        if (summary == null) {
            var newOne = new ProjectCodeAndTime(projectId, time);
            report.AcceptedWork.Add(newOne);
            summary = newOne;
        }

        summary.Time = time;
        Store(report, origin);
    }

    public void CloseProject(string projectId) {
        var projects = GetAllProjects().ToList();
        foreach (var p
                 in projects.Where(p => p.Id == projectId)) {
            p.IsActive = false;
            break;
        }

        File.WriteAllText(Path.Combine(_dataRoot, "activities.json"),
            JsonSerializer.Serialize(new ProjectsContainer {
                Projects = projects
            }));
    }

    public IEnumerable<Project> GetActiveProjects() {
        return GetAllProjects()
            .Where(it => it.IsActive);
    }

    public IEnumerable<Project> GetManagedProjects(string manager) {
        return GetAllProjects()
            .Where(it => it.Manager == manager);
    }

    public Project? GetProjectById(string id) {
        return GetAllProjects().FirstOrDefault(it => it.Id == id);
    }

    public Project CreateProject(CreateProjectDto dto) {
        var projects = GetAllProjects().ToList();
        var project = projects.FirstOrDefault(p => p.Id == dto.Code);
        if (project != null) return project;

        var split = dto.SubprojectCodes.Split(
            ',',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var subprojects = (from code in split select new Subproject {Id = code}).ToList();
        project = new Project {
            Name = dto.ProjectName,
            Id = dto.Code,
            IsActive = true,
            Manager = dto.Manager,
            Subprojects = subprojects,
            Budget = dto.Budget
        };
        projects.Add(project);
        File.WriteAllText(Path.Combine(_dataRoot, "activities.json"),
            JsonSerializer.Serialize(new ProjectsContainer {
                Projects = projects
            }));
        return project;
    }

    public IEnumerable<Project> GetAllProjects() {
        return Directory.EnumerateFiles(
                _dataRoot, "activities.json", SearchOption.TopDirectoryOnly)
            .SelectMany(DeserializeProjects);
    }

    private void Store(MonthReport report, ReportOrigin origin) {
        var path = Path.Combine(_dataRoot, "activities", JsonReportService.GetReportFileName(origin));
        File.WriteAllText(path, JsonSerializer.Serialize(report));
    }

    private int CalcOverallAcceptedTime(string projectCode) {
        return _reportService.GetAllReports()
            .SelectMany(it => it.AcceptedWork)
            .Where(it => it.Id == projectCode)
            .Select(it => it.Time)
            .Sum();
    }

    private static void CopyDataFromDto(Activity activity, EditActivityDto dto) {
        activity.ProjectCode = dto.ProjectCode;
        activity.Description = dto.Description;
        activity.Time = dto.SpentTime;
        activity.SubprojectCode = dto.SubprojectCode;
    }

    private static int GetNextId(IEnumerable<Activity> activities) {
        return activities.Select(activity => activity.Id).Prepend(0).Max() + 1;
    }

    private static List<Project> DeserializeProjects(string path) {
        using var reader = File.OpenText(path);
        var container = JsonSerializer.Deserialize<ProjectsContainer>(reader.ReadToEnd(),
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            });
        return container == null
            ? new List<Project>()
            : container.Projects;
    }
}

internal class ProjectsContainer {
    [JsonPropertyName("activities")] public List<Project> Projects { get; set; } = new();
}

public class ReportOriginWithMeta : ReportOrigin {
    public int Time { get; set; }
    public int AcceptedTime { get; set; }
    public bool IsFrozen { get; set; }

    public override string ToString() {
        return
            $"{nameof(UserName)}: {UserName}, {nameof(Year)}: {Year}, {nameof(Month)}: {Month}, {nameof(Time)}: {Time}, {nameof(AcceptedTime)}: {AcceptedTime}";
    }
}
}