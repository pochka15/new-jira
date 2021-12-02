using System.Text.Json.Serialization;

namespace lab1.Models {
public class ProjectTimeSummary {
    public ProjectTimeSummary(string projectCode = "", int time = 0) {
        ProjectCode = projectCode;
        Time = time;
    }

    public int Id { get; set; }

    [JsonPropertyName("Code")] public string ProjectCode { get; set; }

    public int Time { get; set; }
}
}