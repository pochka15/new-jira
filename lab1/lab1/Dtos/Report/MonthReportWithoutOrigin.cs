using System.Collections.Generic;
using System.Text.Json.Serialization;
using lab1.Models;

namespace lab1.Dtos.Report {
public class MonthReportWithoutOrigin {
    [JsonPropertyName("Frozen")] public bool IsFrozen { get; set; }

    [JsonPropertyName("Entries")] public List<Activity> Activities { get; set; }

    [JsonPropertyName("Accepted")] public List<ProjectCodeAndTime> AcceptedWork { get; set; }
}
}