using System.Text.Json.Serialization;

namespace lab1.Models {
public class AcceptedWork {
    public AcceptedWork(string projectId = "", int spentTime = 0) {
        ProjectId = projectId;
        SpentTime = spentTime;
    }

    [JsonPropertyName("Code")] public string ProjectId { get; set; }

    public int SpentTime { get; set; }
}
}