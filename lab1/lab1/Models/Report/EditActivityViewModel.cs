namespace lab1.Models {
public class EditActivityViewModel {
    public string ProjectCode { get; set; }
    public string SubprojectCode { get; set; }
    public int SpentTime { get; set; }
    public string Description { get; set; }
    public int ActivityId { get; set; }
    public ReportOrigin ReportOrigin { get; set; }
}
}