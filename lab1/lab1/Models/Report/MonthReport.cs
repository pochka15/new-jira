using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using lab1.Dtos.Report;

namespace lab1.Models {
public class MonthReport {
    [JsonIgnore] public int Id { get; set; }
    [JsonIgnore] public string UserName { get; set; }
    [JsonIgnore] public int Year { get; set; }
    [JsonIgnore] public int Month { get; set; }
    [JsonPropertyName("Frozen")] public bool IsFrozen { get; set; }
    [JsonPropertyName("Entries")] public List<Activity> Activities { get; set; }
    [JsonPropertyName("Accepted")] public List<AcceptedWork> AcceptedWorks { get; set; }
    [Timestamp] public byte[] RowVersion { get; set; }


    public MonthReportWithOrigin ToMonthReportWithOrigin() {
        return new MonthReportWithOrigin {
            IsFrozen = IsFrozen,
            Activities = Activities.Select(it => it.ToActivityDto()).ToList(),
            AcceptedWork = AcceptedWorks,
            Origin = new ReportOrigin {
                UserName = UserName,
                Year = Year,
                Month = Month
            }
        };
    }
}
}