using Microsoft.AspNetCore.Mvc;

namespace YourProject.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(string name)
        {
            ViewBag.UserName = name; // هنبعت الاسم من اللوجين
            return View();
        }
    }
}
