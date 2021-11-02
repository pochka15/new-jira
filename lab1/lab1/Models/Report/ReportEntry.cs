using System.Text.Json.Serialization;

namespace lab1.Models {
public class ReportEntry {
    public int Id { get; set; }
    public string Date { get; set; }

    [JsonPropertyName("code")] public string ActivityCode { get; set; }

    public string SubCode { get; set; }
    public int Time { get; set; }
    public string Description { get; set; }
}
}