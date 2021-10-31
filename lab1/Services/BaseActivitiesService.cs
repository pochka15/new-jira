#nullable enable
using System.IO;
using System.Linq;
using System.Text.Json;
using lab1.Models;

namespace lab1.Services {
public class BaseActivitiesService : IActivitiesService {
    private readonly string _pathToData;

    public BaseActivitiesService(string pathToData) {
        _pathToData = pathToData;
    }

    // TODO(@pochka15): test it
    public Activity? GetActivityByCode(string code) {
        var path = Path.Combine(_pathToData, "data");
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