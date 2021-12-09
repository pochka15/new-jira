using System.Collections.Generic;
using lab1.Dtos.Others;
using lab1.Dtos.Project;
using lab1.Dtos.Report;

namespace lab1.Models {
public class ReportViewModel {
    public bool Frozen { get; set; }
    public List<ActivityDto> Activities { get; set; }
    public ReportOrigin Origin { get; set; }
    public int ActivityId { get; set; }
    public ChangeDateForm ChangeDateForm { get; set; }
    public int OverallTime { get; set; }
    public string UserName { get; set; }
    public bool CanAddActivity { get; set; }

    // Used only for POST
    public int Year { get; set; }
    public int Month { get; set; }
}
}