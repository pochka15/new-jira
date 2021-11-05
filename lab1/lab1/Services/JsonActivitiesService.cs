#nullable enable
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using lab1.Models;

namespace lab1.Services {
public class JsonActivitiesService : IActivitiesService {
    private readonly string _dataRoot;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="dataRoot">a root folder containing the json data</param>
    public JsonActivitiesService(string dataRoot) {
        _dataRoot = dataRoot;
    }

    public Project? GetProjectByCode(string code) {
        var path = Path.Combine(_dataRoot);
        return Directory.EnumerateFiles(path, "activities.json", SearchOption.TopDirectoryOnly)
            .SelectMany(DeserializeProjects)
            .FirstOrDefault(it => it.Code == code);
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