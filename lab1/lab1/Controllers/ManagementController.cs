using System.Collections.Generic;
using System.Linq;
using lab1.Extensions;
using lab1.Models;
using lab1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers {
public class ManagementController : Controller {
    private readonly IProjectService _projectService;

    public ManagementController(IProjectService projectService) {
        _projectService = projectService;
    }

    private SessionState SessionState {
        get {
            var userName = HttpContext.Session.GetString(SessionState.UserNameField);
            var year = HttpContext.Session.GetString(SessionState.YearField);
            var month = HttpContext.Session.GetString(SessionState.MonthField);
            var day = HttpContext.Session.GetString(SessionState.DayField);

            return new SessionState {
                UserName = userName,
                Year = year != null ? int.Parse(year) : null,
                Month = month != null ? int.Parse(month) : null,
                Day = day != null ? int.Parse(day) : null
            };
        }
    }

    public IActionResult Index() {
        var manager = SessionState.UserName;
        var projects = manager == null
            ? new List<SimplifiedProject>()
            : _projectService.GetManagedProjects(manager)
                .Select(it => it.ToSimplifiedProject())
                .ToList();
        var model = new ManagementModelView {Projects = projects};
        return View(model);
    }
}
}