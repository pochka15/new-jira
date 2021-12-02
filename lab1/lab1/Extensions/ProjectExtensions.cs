using lab1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace lab1.Extensions {
public static class ProjectExtensions {
    public static SelectListItem ToSelectItem(this Project project) {
        return new SelectListItem(project.Name, project.Code);
    }

    public static SimplifiedProject ToSimplifiedProject(this Project project) {
        return new SimplifiedProject {
            Code = project.Code,
            Name = project.Name
        };
    }
}
}