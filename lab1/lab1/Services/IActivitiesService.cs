#nullable enable
using lab1.Models;

namespace lab1.Services {
public interface IActivitiesService {
    Project? GetProjectByCode(string code);
}
}