#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using lab1.Dtos.Others;
using lab1.Dtos.Report;
using lab1.Models;
using lab1.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace lab1.Services {
public class DbReportService : IReportService {
    private readonly MainContext _ctx;

    public DbReportService(MainContext ctx) {
        _ctx = ctx;
    }

    public DayActivities? GetDayReport(ReportOrigin origin, int day) {
        var report = RepositoryUtils.GetReportWithActivities(origin, _ctx);
        if (report == null) return null;

        return new DayActivities {
            Frozen = report.IsFrozen,
            Activities = report.Activities
                .Select(it => it.ToActivityDto())
                .Where(it => DateTime.Parse(it.Date).Day == day)
                .ToList()
        };
    }


    public MonthReportWithOrigin? GetMonthReport(ReportOrigin origin) {
        var report = RepositoryUtils.GetReportWithActivitiesAndAcceptedWork(origin, _ctx);
        return report?.ToMonthReportWithOrigin();
    }

    public IEnumerable<MonthReportWithOrigin> GetAllReports() {
        return _ctx.MonthReports
            .Include(it => it.Activities)
            .Include(it => it.AcceptedWorks)
            .Select(it => it.ToMonthReportWithOrigin())
            .ToList();
    }

    public MonthStatistics GetMonthStatistics(ReportOrigin origin) {
        var report = GetMonthReport(origin);
        if (report == null) return new MonthStatistics();
        return new MonthStatistics {
            ProjectToTime = JsonReportService.BuildProjectToTime(report),
            ProjectToAcceptedTime = JsonReportService.GetProjectToAcceptedTime(report)
        };
    }

    public MonthReportWithOrigin CreateBlankReport(ReportOrigin origin) {
        var report = RepositoryUtils.GetReport(origin, _ctx);
        if (report != null) return report.ToMonthReportWithOrigin();

        report = new MonthReport {
            UserName = origin.UserName,
            Year = origin.Year,
            Month = origin.Month,
            Activities = new List<Activity>(),
            IsFrozen = false,
            AcceptedWorks = new List<AcceptedWork>(),
        };
        _ctx.MonthReports.Add(report);
        _ctx.SaveChanges();
        return report.ToMonthReportWithOrigin();
    }

    public void SubmitMonthActivities(ReportOrigin origin) {
        var report = RepositoryUtils.GetReport(origin, _ctx);
        if (report == null) return;
        report.IsFrozen = true;
        _ctx.SaveChanges();
    }

    public IEnumerable<ReportOriginWithMeta> GetReportOriginsWithMeta(string projectId) {
        return GetAllReports()
            .Select(it => {
                it.Activities = it.Activities
                    .Where(activity => activity.ProjectCode == projectId)
                    .ToList();
                return it;
            })
            .Where(it => it.Activities.Any())
            .Select(it => it.ToReportOriginWithMeta(projectId));
    }
}
}