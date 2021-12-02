using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lab1.Models {
public class Project {
    [JsonPropertyName("Code")] public string Id { get; set; }
    [JsonPropertyName("Owner")] public string Manager { get; set; }
    public string Name { get; set; }
    public int Budget { get; set; }
    [JsonPropertyName("Active")] public bool IsActive { get; set; }
    public int Cost { get; set; }
    [JsonPropertyName("SubActivities")] public List<Subproject> Subprojects { get; set; }
}
}