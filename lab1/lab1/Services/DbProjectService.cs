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
            .Select(it => it.ToProjectDto());
    }

    public IEnumerable<ProjectDto> GetManagedProjects(string manager) {
        return _ctx.Projects
            .Where(it => it.Manager == manager)
            .Select(it => it.ToProjectDto());
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
        throw new NotImplementedException();
    }

    public void EditActivity(ReportOrigin reportOrigin, EditActivityDto dto) {
        throw new NotImplementedException();
    }

    public void AddActivity(ReportOrigin origin, AddActivityDto dto) {
        var isActive = GetProjectById(dto.ProjectCode)!.IsActive;
        Debug.Assert(isActive);

        // _ctx.MonthReports
        // .Where(it => it.)
        // var report = _reportService.GetMonthReport(origin) ?? _reportService.CreateBlankReport(origin);
        // report.Activities
        // .Add(new Activity {
        // Date = string.Join('/', origin.Year, origin.Month, dto.Day),
        // ProjectCode = dto.ProjectCode,
        // SubprojectCode = dto.SubprojectCode,
        // Time = dto.SpentTime,
        // Description = dto.Description
        // });
    }

    public void UpdateCost(string projectId, int cost) {
        throw new NotImplementedException();
    }

    public int CalcLeftBudget(ProjectDto project) {
        throw new NotImplementedException();
    }

    public void AcceptTime(ReportOrigin origin, string projectId, int time) {
        throw new NotImplementedException();
    }

    public void CloseProject(string projectId) {
        throw new NotImplementedException();
    }

    public IEnumerable<ProjectDto> GetAllProjects() {
        throw new NotImplementedException();
    }
}
}