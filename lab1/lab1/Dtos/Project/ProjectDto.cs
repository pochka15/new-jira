using System.Collections.Generic;
using System.Linq;
using lab1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace lab1.Dtos.Project {
public class ProjectDto {
    public string Id { get; set; }
    public string Manager { get; set; }
    public string Name { get; set; }
    public int Budget { get; set; }
    public bool IsActive { get; set; }
    public int Cost { get; set; }
    public List<SubprojectDto> Subprojects { get; set; }

    public SelectListItem ToSelectItem() {
        return new SelectListItem(Name, Id);
    }

    public SimplifiedProject ToSimplifiedProject() {
        return new SimplifiedProject {Code = Id, Name = Name};
    }

    public Models.Project ToModel() {
        return new Models.Project {
            Id = Id,
            Manager = Manager,
            Name = Name,
            Budget = Budget,
            IsActive = IsActive,
            Cost = Cost,
            Subprojects = Subprojects.Select(x => x.ToModel()).ToList()
        };
    }
}
}