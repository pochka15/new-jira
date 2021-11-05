#nullable enable
using lab1.Models;

namespace lab1.Services {
public interface IProjectsService {
    Project? GetProjectByCode(string code);
    Project CreateProject(CreateProjectDto dto);
}
}