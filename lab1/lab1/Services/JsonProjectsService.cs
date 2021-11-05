#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using lab1.Models;

namespace lab1.Services {
public class JsonProjectsService : IProjectsService {
    private readonly string _dataRoot;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="dataRoot">a root folder containing the json data</param>
    public JsonProjectsService(string dataRoot) {
        _dataRoot = dataRoot;
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