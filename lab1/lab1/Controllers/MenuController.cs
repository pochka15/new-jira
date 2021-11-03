using Microsoft.AspNetCore.Mvc;

namespace lab1.Controllers {
public class MenuController : Controller {
    // GET
    public IActionResult Index() {
        return View();
    }
}
}