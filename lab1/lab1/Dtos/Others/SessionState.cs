#nullable enable
namespace lab1.Dtos.Others {
public class SessionState {
    public const string UserNameField = "UserName";
    public const string YearField = "Year";
    public const string MonthField = "Month";
    public const string DayField = "Day";
    public string? UserName { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }

    public bool HasNullFields => UserName == null || Year == null || Month == null || Day == null;
}
}