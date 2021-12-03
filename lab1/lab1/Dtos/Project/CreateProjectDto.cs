namespace lab1.Dtos.Project {
public class CreateProjectDto {
    public string ProjectName { get; set; }
    public string Code { get; set; }
    public int Budget { get; set; }
    public string SubprojectCodes { get; set; }
    public string Manager { get; set; }
}
}