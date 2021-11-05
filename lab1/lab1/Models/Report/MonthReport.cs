using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lab1.Models {
public class MonthReport {
    public bool Frozen { get; set; }

    [JsonPropertyName("Entries")] public List<Activity> Activities { get; set; }

    [JsonPropertyName("Accepted")] public List<ActivityTime> AcceptedActivities { get; set; }
}
}