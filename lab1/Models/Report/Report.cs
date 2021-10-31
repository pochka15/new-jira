using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lab1.Models {
public class Report {
    public bool Frozen { get; set; }

    public List<ReportEntry> Entries { get; set; }

    [JsonPropertyName("accepted")] public List<ActivityTime> AcceptedActivities { get; set; }
}
}