using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lab1.Models {
public class MonthReport {
    public int Id { get; set; }
    [JsonPropertyName("Frozen")] public bool IsFrozen { get; set; }

    [JsonPropertyName("Entries")] public List<Activity> Activities { get; set; }

    [JsonPropertyName("Accepted")] public List<ProjectTimeSummary> TimeSummaries { get; set; }
}
}