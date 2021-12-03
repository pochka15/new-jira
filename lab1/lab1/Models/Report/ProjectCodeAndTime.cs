using System.Text.Json.Serialization;

namespace lab1.Models {
public class ProjectCodeAndTime {
    public ProjectCodeAndTime(string id = "", int time = 0) {
        Id = id;
        Time = time;
    }

    [JsonPropertyName("Code")] public string Id { get; set; }

    public int Time { get; set; }
}
}