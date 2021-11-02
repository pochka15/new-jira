#nullable enable
using System.IO;
using System.Linq;
using System.Text.Json;
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

    public Activity? GetActivityByCode(string code) {
        var path = Path.Combine(_dataRoot);
        return Directory.EnumerateFiles(path, "activities.json", SearchOption.TopDirectoryOnly)
            .SelectMany(it => DeserializeActivitiesContainer(it).Activities)
            .FirstOrDefault(it => it != null && it.Code == code);
    }

    private ActivitiesContainer DeserializeActivitiesContainer(string path) {
        using var reader = File.OpenText(path);
        var wrapper = JsonSerializer.Deserialize<ActivitiesContainer>(reader.ReadToEnd(),
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            });
        return wrapper!;
    }
}
}