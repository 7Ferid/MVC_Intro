using Microsoft.AspNetCore.Mvc;

namespace MVC_Intro.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
