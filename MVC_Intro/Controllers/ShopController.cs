using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Intro.Contexts;
using MVC_Intro.ViewModels.ProductViewModels;

namespace MVC_Intro.Controllers
{
    public class ShopController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
     


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products.Select(x => new ProductGetVM()
            {
                Id = x.Id,
                Name = x.Name,

                Description = x.Description,
                AdditionalImagePaths = x.ProductImages.Select(x => x.ImagePath).ToList(),
                CategoryName = x.Category.Name,
                HoverImagePath = x.HoverImagePath,
                MainImagePath = x.MainImagePath,
                Price = x.Price,
                TagNames = x.ProductTags.Select(x => x.Tag.Name).ToList()

            }).FirstOrDefaultAsync(x => x.Id == id);
            if (product is null) return NotFound();

            return View(product);

        }
    }
}





