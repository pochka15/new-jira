namespace lab1.Dtos.Report {
public class ReportOrigin {
    public string UserName { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }

    public override string ToString() {
        return
            $"{nameof(UserName)}: {UserName}, {nameof(Year)}: {Year}, {nameof(Month)}: {Month}";
    }
}
}