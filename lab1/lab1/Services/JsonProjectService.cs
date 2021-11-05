#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using lab1.Models;

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
        var report = _reportService.GetMonthReport(origin)!;
        var path = Path.Combine(_dataRoot, "activities", JsonReportService.GetReportFileName(origin));

        foreach (var activity in report.Activities.Where(it => it.Id == dto.Id)) {
            CopyDataFromDto(activity, dto);
            break;
        }

        File.WriteAllText(path, JsonSerializer.Serialize(report));
    }

    public void DeleteActivityMatching(ReportOrigin origin, Predicate<Activity> pred) {
        var report = _reportService.GetMonthReport(origin)!;
        var path = Path.Combine(_dataRoot, "activities", JsonReportService.GetReportFileName(origin));

        report.Activities.RemoveAll(pred);
        File.WriteAllText(path, JsonSerializer.Serialize(report));
    }

    public void AddActivity(ReportOrigin origin, AddActivityDto dto) {
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


    public Project? GetProjectByCode(string code) {
        return GetAllProjects().FirstOrDefault(it => it.Code == code);
    }

    public Project CreateProject(CreateProjectDto dto) {
        var projects = GetAllProjects().ToList();
        var project = projects.FirstOrDefault(p => p.Code == dto.Code);
        if (project != null) return project;

        var split = dto.SubprojectCodes.Split(
            ',',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var subprojects = (from code in split select new Subproject {Code = code}).ToList();
        project = new Project {
            Name = dto.ProjectName,
            Code = dto.Code,
            Active = true,
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

    private static void CopyDataFromDto(Activity activity, EditActivityDto dto) {
        activity.ProjectCode = dto.ProjectCode;
        activity.Description = dto.Description;
        activity.Time = dto.SpentTime;
        activity.SubprojectCode = dto.SubprojectCode;
    }

    private static int GetNextId(IEnumerable<Activity> activities) {
        return activities.Select(activity => activity.Id).Prepend(0).Max() + 1;
    }

    private IEnumerable<Project> GetAllProjects() {
        return Directory.EnumerateFiles(
                _dataRoot, "activities.json", SearchOption.TopDirectoryOnly)
            .SelectMany(DeserializeProjects);
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
}