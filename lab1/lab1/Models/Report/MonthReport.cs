using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lab1.Models {
public class MonthReport {
    public bool Frozen { get; set; }

    public List<Activity> Entries { get; set; }

    [JsonPropertyName("accepted")] public List<ActivityTime> AcceptedActivities { get; set; }
}
}