using System.Collections.Generic;

namespace lab1.Models {
public class DayReport {
    public bool Frozen { get; set; }
    public List<ReportEntry> Entries { get; set; }
}
}