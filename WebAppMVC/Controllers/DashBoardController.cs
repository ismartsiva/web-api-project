using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
