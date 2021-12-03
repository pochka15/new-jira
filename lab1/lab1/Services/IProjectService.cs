#nullable enable
using System;
using System.Collections.Generic;
using lab1.Dtos.Project;
using lab1.Dtos.Report;
using lab1.Models;

namespace lab1.Services {
public interface IProjectService {
    ProjectDto? GetProjectById(string id);
    IEnumerable<ProjectDto> GetActiveProjects();
    IEnumerable<ProjectDto> GetManagedProjects(string manager);
    void CreateProject(CreateProjectDto dto);
    void DeleteActivityMatching(ReportOrigin reportOrigin, Predicate<Activity> pred);
    void EditActivity(ReportOrigin reportOrigin, EditActivityDto dto);
    void AddActivity(ReportOrigin origin, AddActivityDto dto);
    void UpdateCost(string projectId, int cost);
    int CalcLeftBudget(ProjectDto project);
    void AcceptTime(ReportOrigin origin, string projectId, int time);
    void CloseProject(string projectId);
    IEnumerable<ProjectDto> GetAllProjects();
}
}