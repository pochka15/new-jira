using System.Collections.Generic;

namespace lab1.Models {
public class ReportViewModel {
    public bool Frozen { get; set; }
    public List<Activity> Entries { get; set; }
    public ReportOrigin Origin { get; set; }
    public int ActivityId { get; set; }
    public ReportOrigin ReportOrigin { get; set; }
    public ChangeDateForm ChangeDateForm { get; set; }
    public int OverallTime { get; set; }
    public string UserName { get; set; }
}
}