using Microsoft.AspNetCore.Mvc;
using MVC_Intro.Contexts;

namespace MVC_Intro.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var sliders=_context.Sliders.ToList();
            //ViewBag.Sliders = sliders;

            return View(sliders);
        }
    }
}
