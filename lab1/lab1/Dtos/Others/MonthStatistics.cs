using System.Collections.Generic;

namespace lab1.Dtos.Others {
public class MonthStatistics {
    public Dictionary<string, int> ProjectToTime { get; set; } = new();
    public Dictionary<string, int> ProjectToAcceptedTime { get; set; } = new();
}
}