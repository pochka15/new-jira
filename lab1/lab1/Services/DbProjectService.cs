#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using lab1.Dtos.Project;
using lab1.Dtos.Report;
using lab1.Models;
using lab1.Models.Data;
using Activity = lab1.Models.Activity;

namespace lab1.Services {
public class DbProjectService : IProjectService {
    private readonly MainContext _ctx;
    private readonly IReportService _reportService;

    public DbProjectService(MainContext ctx, IReportService reportService) {
        _ctx = ctx;
        _reportService = reportService;
    }

    public ProjectDto? GetProjectById(string id) {
        return _ctx.Projects
            .FirstOrDefault(it => it.Id == id)
            ?.ToProjectDto();
    }

    public IEnumerable<ProjectDto> GetActiveProjects() {
        return _ctx.Projects
            .Where(it => it.IsActive)
            .Select(it => it.ToProjectDto())
            .ToList();
    }

    public IEnumerable<ProjectDto> GetManagedProjects(string manager) {
        return _ctx.Projects
            .Where(it => it.Manager == manager)
            .Select(it => it.ToProjectDto())
            .ToList();
    }

    public void CreateProject(CreateProjectDto dto) {
        var project = _ctx.Projects
            .FirstOrDefault(it => it.Id == dto.Code);
        if (project != null) return;

        var split = dto.SubprojectCodes.Split(
            ',',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var subprojects = (from code in split select new Subproject {Id = code}).ToList();
        project = new Project {
            Name = dto.ProjectName,
            Id = dto.Code,
            IsActive = true,
            Manager = dto.Manager,
            Subprojects = subprojects,
            Budget = dto.Budget
        };

        _ctx.Projects.Add(project);
        _ctx.SaveChanges();
    }

    public void DeleteActivityMatching(ReportOrigin reportOrigin, Predicate<Activity> pred) {
        var report = RepositoryUtils.GetReportWithActivities(reportOrigin, _ctx);
        var entity = report?.Activities.FirstOrDefault(pred.Invoke);
        if (entity == null) return;
        _ctx.Activities.Remove(entity);
        _ctx.SaveChanges();
    }

    // TODO(@pochka15): test it
    public void EditActivity(ReportOrigin reportOrigin, EditActivityDto dto) {
        var activity = RepositoryUtils.GetReportWithActivities(reportOrigin, _ctx)
            ?.Activities
            .FirstOrDefault(it => it.Id == dto.Id);
        if (activity == null) return;

        activity.ProjectCode = dto.ProjectCode;
        activity.SubprojectCode = dto.SubprojectCode;
        activity.Time = dto.SpentTime;
        activity.Description = dto.Description;
        _ctx.SaveChanges();
    }

    public void AddActivity(ReportOrigin origin, AddActivityDto dto) {
        var isActive = GetProjectById(dto.ProjectCode)!.IsActive;
        Debug.Assert(isActive);

        var report = RepositoryUtils.GetReportWithActivities(origin, _ctx);
        // ReSharper disable once ConvertIfStatementToNullCoalescingExpression
        if (report == null) {
            // Note: it can be optimized by creating the report in the repository
            var reportDto = _reportService.CreateBlankReport(origin);
            report = RepositoryUtils.GetReport(reportDto.Origin, _ctx)!;
        }

        report.Activities
            .Add(new Activity {
                Date = string.Join('/', origin.Year, origin.Month, dto.Day),
                ProjectCode = dto.ProjectCode,
                SubprojectCode = dto.SubprojectCode,
                Time = dto.SpentTime,
                Description = dto.Description
            });
        _ctx.SaveChanges();
    }

    public void UpdateCost(string projectId, int cost) {
        var project = FindProjectById(projectId);
        if (project == null) return;
        project.Cost = cost;
        _ctx.SaveChanges();
    }

    public int CalcLeftBudget(ProjectDto project) {
        return project.Budget - CalcOverallAcceptedTime(project.Id) * project.Cost;
    }

    public void AcceptTime(ReportOrigin origin, string projectId, int time) {
        var report = RepositoryUtils.GetReportWithAcceptedWork(origin, _ctx);
        if (report == null) return;

        var summary = report.AcceptedWork
            .FirstOrDefault(it => it.Id == projectId);
        if (summary == null) {
            var newOne = new ProjectCodeAndTime(projectId, time);
            report.AcceptedWork.Add(newOne);
            summary = newOne;
        }

        summary.Time = time;
        _ctx.SaveChanges();
    }

    public void CloseProject(string projectId) {
        var project = _ctx.Projects
            .FirstOrDefault(it => it.Id == projectId);
        if (project == null) return;
        project.IsActive = false;
        _ctx.SaveChanges();
    }

    public IEnumerable<ProjectDto> GetAllProjects() {
        return _ctx.Projects.Select(it => it.ToProjectDto()).ToList();
    }

    private Project? FindProjectById(string id) {
        return _ctx.Projects
            .FirstOrDefault(it => it.Id == id);
    }

    private int CalcOverallAcceptedTime(string projectCode) {
        return _reportService.GetAllReports()
            .SelectMany(it => it.AcceptedWork)
            .Where(it => it.Id == projectCode)
            .Select(it => it.Time)
            .Sum();
    }
}
}