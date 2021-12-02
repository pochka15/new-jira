using System.Collections.Generic;
using System.Linq;

namespace lab1.Models.Data {
public static class DbInitializer {
    public static void Initialize(MainContext context) {
        context.Database.EnsureCreated();

        var isDbSeeded = context.Projects.Any();
        if (isDbSeeded) return;

        var project = new Project {
            Manager = "Some manager",
            Name = "Test project",
            Budget = 100,
            IsActive = true,
            Cost = 1,
            Subprojects = new List<Subproject>()
        };
        context.Projects.Add(project);
        context.SaveChanges();
    }
}
}