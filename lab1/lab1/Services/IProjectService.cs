#nullable enable
using System;
using lab1.Models;

namespace lab1.Services {
public interface IProjectService {
    Project? GetProjectByCode(string code);
    Project CreateProject(CreateProjectDto dto);
    void DeleteActivityMatching(ReportOrigin reportOrigin, Predicate<Activity> pred);
    void EditActivity(ReportOrigin reportOrigin, EditActivityDto dto);
    void AddActivity(ReportOrigin origin, AddActivityDto dto);
    void UpdateCost(string projectCode, int cost);
}
}