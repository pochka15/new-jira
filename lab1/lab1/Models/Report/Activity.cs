using System.Text.Json.Serialization;
using lab1.Dtos.Project;

namespace lab1.Models {
public class Activity {
    public int Id { get; set; }
    public string Date { get; set; }

    [JsonPropertyName("Code")] public string ProjectCode { get; set; }

    [JsonPropertyName("Subcode")] public string SubprojectCode { get; set; }
    public int Time { get; set; }
    public string Description { get; set; }

    public ActivityDto ToActivityDto() {
        return new ActivityDto {
            Id = Id,
            Date = Date,
            ProjectCode = ProjectCode,
            SubprojectCode = SubprojectCode,
            Time = Time,
            Description = Description
        };
    }
}
}