#nullable enable
using System;
using System.Collections.Generic;
using lab1.Dtos.Project;
using lab1.Dtos.Report;
using lab1.Models;

namespace lab1.Services {
public interface IProjectService {
    Project? GetProjectById(string id);
    IEnumerable<Project> GetActiveProjects();
    IEnumerable<Project> GetManagedProjects(string manager);
    Project CreateProject(CreateProjectDto dto);
    void DeleteActivityMatching(ReportOrigin reportOrigin, Predicate<Activity> pred);
    void EditActivity(ReportOrigin reportOrigin, EditActivityDto dto);
    void AddActivity(ReportOrigin origin, AddActivityDto dto);
    void UpdateCost(string projectId, int cost);
    int CalcLeftBudget(Project project);
    void AcceptTime(ReportOrigin origin, string projectId, int time);
    void CloseProject(string projectId);
    IEnumerable<Project> GetAllProjects();
}
}