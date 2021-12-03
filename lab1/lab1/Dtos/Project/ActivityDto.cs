using lab1.Models;

namespace lab1.Dtos.Project {
public class ActivityDto {
    public int Id { get; set; }
    public string Date { get; set; }
    public string ProjectCode { get; set; }
    public string SubprojectCode { get; set; }
    public int Time { get; set; }
    public string Description { get; set; }

    public Activity ToModel() {
        return new Activity {
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