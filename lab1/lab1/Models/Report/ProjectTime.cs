using System.Text.Json.Serialization;

namespace lab1.Models {
public class ProjectTime {
    [JsonPropertyName("Code")] public string ProjectCode { get; set; }

    public int Time { get; set; }
}
}