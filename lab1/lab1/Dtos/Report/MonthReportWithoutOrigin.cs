using System.Collections.Generic;
using lab1.Dtos.Project;
using lab1.Models;

namespace lab1.Dtos.Report {
public class MonthReportWithoutOrigin {
    public bool IsFrozen { get; set; }

    public List<ActivityDto> Activities { get; set; }

    public List<AcceptedWork> AcceptedWork { get; set; }
}
}