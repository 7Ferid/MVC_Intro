using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Intro.Abstraction;
using MVC_Intro.Contexts;
using MVC_Intro.ViewModels.ProductViewModels;
using System.Threading.Tasks;

namespace MVC_Intro.Controllers
{
    public class ShopController(AppDbContext _context,IEmailService _emailService) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
     
        public async Task<IActionResult> Test()
        {
            await _emailService.SendEmailAsync("faridgg-mpa101@code.edu.az", "MPA101", "Email service is done");


             return Ok("Ok");   
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





