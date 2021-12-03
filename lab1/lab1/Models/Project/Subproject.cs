using System.Text.Json.Serialization;
using lab1.Dtos.Project;

namespace lab1.Models {
public class Subproject {
    [JsonPropertyName("Code")] public string Id { get; set; }

    public SubprojectDto ToSubProjectDto() {
        return new SubprojectDto {
            Id = Id
        };
    }
}
}