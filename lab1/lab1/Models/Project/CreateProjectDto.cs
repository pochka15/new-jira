namespace lab1.Models {
public class CreateProjectDto {
    public string ProjectName { get; set; }
    public string Code { get; set; }
    public int Budget { get; set; }
    public string SubProjects { get; set; }
    public string Manager { get; set; }
}
}