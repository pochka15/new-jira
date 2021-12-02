using System.Text.Json.Serialization;

namespace lab1.Models {
public class Subproject {
    [JsonPropertyName("Code")] public string Id { get; set; }
}
}