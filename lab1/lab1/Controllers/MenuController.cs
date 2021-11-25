using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers {
/// <summary>
/// Controller that represents the context menu
/// </summary>
public class MenuController : Controller {
    public IActionResult Index() {
        return View();
    }
}
}