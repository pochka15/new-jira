#nullable enable
namespace lab1.Dtos.Report {
public class AddActivityDto {
    public string ProjectCode { get; set; } = "";
    public string? SubprojectCode { get; set; }
    public int SpentTime { get; set; }
    public string Description { get; set; } = "";
    public int Day { get; set; }
}
}