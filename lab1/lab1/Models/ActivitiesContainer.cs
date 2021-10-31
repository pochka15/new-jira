using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lab1.Models {
public class ActivitiesContainer {
    [JsonPropertyName("activities")] public List<Activity> Activities { get; set; }
}
}