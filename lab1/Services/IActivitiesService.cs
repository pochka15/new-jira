#nullable enable
using lab1.Models;

namespace lab1.Services {
public interface IActivitiesService {
    Activity? GetActivityByCode(string code);
}
}