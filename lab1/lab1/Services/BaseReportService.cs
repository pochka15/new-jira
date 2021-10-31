#nullable enable
using System.IO;
using System.Linq;
using System.Text.Json;
using lab1.Models;
using Microsoft.AspNetCore.Hosting;

namespace lab1.Services {
public class BaseReportService : IReportService {
    private readonly IWebHostEnvironment _environment;

    public BaseReportService(IWebHostEnvironment environment) {
        _environment = environment;
    }

    public Report? GetUserReport(string userName, int year, int month) {
        var root = Path.Combine(_environment.WebRootPath, "data", "activities");
        var files = Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories);
        var query = from path in files
            where Path.GetFileName(path).Equals(userName + "-" + year + "-" + month + ".json")
            select DeserializeReport(path);
        return query.FirstOrDefault();
    }

    private Report DeserializeReport(string path) {
        using var reader = File.OpenText(path);
        return JsonSerializer.Deserialize<Report>(reader.ReadToEnd(),
            new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            })!;
    }
}
}