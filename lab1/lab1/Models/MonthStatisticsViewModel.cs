using System.Collections.Generic;

namespace lab1.Models {
public class MonthStatisticsViewModel {
    public Dictionary<string, int> ProjectToTime { get; set; }
    public string UserName { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}
}