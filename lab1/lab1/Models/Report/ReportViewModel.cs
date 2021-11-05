using System.Collections.Generic;

namespace lab1.Models {
public class ReportViewModel {
    public bool Frozen { get; set; }
    public List<ReportEntry> Entries { get; set; }
    public ReportOrigin Origin { get; set; }
    public EntryDescription EntryDescription { get; set; }
    public ChangeDateForm ChangeDateForm { get; set; }
    public int OverallTime { get; set; }
    public string UserName { get; set; }
}
}