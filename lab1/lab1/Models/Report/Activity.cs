using System.Text.Json.Serialization;

namespace lab1.Models {
public class Activity {
    public int Id { get; set; }
    public string Date { get; set; }

    [JsonPropertyName("Code")] public string ProjectCode { get; set; }

    [JsonPropertyName("Subcode")] public string SubprojectCode { get; set; }
    public int Time { get; set; }
    public string Description { get; set; }
}
}