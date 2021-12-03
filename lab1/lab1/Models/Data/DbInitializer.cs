using System.Linq;
using lab1.Services;

namespace lab1.Models.Data {
public static class DbInitializer {
    public static void Initialize(
        MainContext context,
        IProjectService projectService,
        IReportService reportService) {
        context.Database.EnsureCreated();

        var isDbSeeded = context.Projects.Any();
        if (isDbSeeded) return;

        foreach (var p in projectService.GetAllProjects()) {
            context.Projects.Add(p.ToModel());
        }

        context.SaveChanges();


        foreach (var r in reportService.GetAllReports()) {
            context.MonthReports.Add(r.ToMonthReport());
        }

        context.SaveChanges();
    }
}
}