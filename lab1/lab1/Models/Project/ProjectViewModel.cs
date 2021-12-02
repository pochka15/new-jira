using System.Collections.Generic;
using lab1.Services;

namespace lab1.Models {
public class ProjectViewModel {
    public int Cost { get; set; }
    public string ProjectName { get; set; }
    public string ProjectCode { get; set; }
    public int LeftBudget { get; set; }
    public int InitialBudget { get; set; }
    public IEnumerable<ReportOriginWithMeta> ReportOriginsWithMeta { get; set; }
    public bool ProjectIsActive { get; set; }
}
}