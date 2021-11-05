using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace lab1.Models {
public class Project {
    public string Code { get; set; }
    [JsonPropertyName("Owner")] public string Manager { get; set; }
    public string Name { get; set; }
    public float Budget { get; set; }
    public bool Active { get; set; }
    public int Cost { get; set; }
    [JsonPropertyName("SubActivities")] public List<Subproject> Subprojects { get; set; }
}
}