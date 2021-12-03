using System.Collections.Generic;

namespace lab1.Models {
public class MonthReport {
    public int Id { get; set; }
    public string UserName { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public bool IsFrozen { get; set; }
    public List<Activity> Activities { get; set; }
    public List<ProjectCodeAndTime> AcceptedWork { get; set; }
}
}