#nullable enable
using System.Linq;
using lab1.Dtos.Report;
using lab1.Models;
using lab1.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace lab1.Services {
public static class RepositoryUtils {
    public static MonthReport? GetReportWithActivities(ReportOrigin reportOrigin, MainContext ctx) {
        return ctx.MonthReports
            .Include(it => it.Activities)
            .FirstOrDefault(it => it.Year == reportOrigin.Year
                                  && it.Month == reportOrigin.Month
                                  && it.UserName == reportOrigin.UserName);
    }

    public static MonthReport? GetReportWithAcceptedWork(ReportOrigin reportOrigin, MainContext ctx) {
        return ctx.MonthReports
            .Include(it => it.AcceptedWorks)
            .FirstOrDefault(it => it.Year == reportOrigin.Year
                                  && it.Month == reportOrigin.Month
                                  && it.UserName == reportOrigin.UserName);
    }

    public static MonthReport? GetReport(ReportOrigin origin, MainContext ctx) {
        return ctx.MonthReports
            .FirstOrDefault(it => it.Year == origin.Year
                                  && it.Month == origin.Month
                                  && it.UserName == origin.UserName);
    }

    public static MonthReport? GetReportWithActivitiesAndAcceptedWork(ReportOrigin origin, MainContext ctx) {
        return ctx.MonthReports
            .Include(it => it.Activities)
            .Include(it => it.AcceptedWorks)
            .FirstOrDefault(it => it.Year == origin.Year
                                  && it.Month == origin.Month
                                  && it.UserName == origin.UserName);
    }
}
}