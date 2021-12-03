using System.Text.Json.Serialization;
using lab1.Models;

namespace lab1.Dtos.Project {
public class SubprojectDto {
    [JsonPropertyName("Code")] public string Id { get; set; }

    public Subproject ToModel() {
        return new Subproject {
            Id = Id
        };
    }
}
}