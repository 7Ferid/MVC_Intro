using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Intro.Contexts;

namespace MVC_Intro.Controllers
{
    public class ShopController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var products= await _context.Products.ToListAsync();
            return View(products);
        }
    }
}
