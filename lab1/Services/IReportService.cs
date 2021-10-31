#nullable enable
using lab1.Models;

namespace lab1.Services {
public interface IReportService {
    Report? GetUserReport(string userName, int year, int month);
}
}