using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using lab1.Dtos.Project;

namespace lab1.Models {
public class Project {
    [JsonPropertyName("Code")] public string Id { get; set; }
    [JsonPropertyName("Owner")] public string Manager { get; set; }
    public string Name { get; set; }
    public int Budget { get; set; }
    [JsonPropertyName("Active")] public bool IsActive { get; set; }
    public int Cost { get; set; }
    [JsonPropertyName("SubActivities")] public List<Subproject> Subprojects { get; set; } = new();

    public ProjectDto ToProjectDto() {
        return new ProjectDto {
            Id = Id,
            Manager = Manager,
            Name = Name,
            Budget = Budget,
            IsActive = IsActive,
            Cost = Cost,
            Subprojects = Subprojects.Select(it => it.ToSubProjectDto()).ToList()
        };
    }
}
}