using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Intro.Contexts;
using MVC_Intro.Models;
using System.Threading.Tasks;

namespace MVC_Intro.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var products = await _context.Products.Include(x => x.Category).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            await SendCategoriesWithViewBag();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {

          
            if (!ModelState.IsValid)
            {  await SendCategoriesWithViewBag();
                return View(product);
            }
            var isExistCategory= await _context.Products.AnyAsync(x=>x.Id==product.CategoryId);

            if(!isExistCategory)
            {
                await SendCategoriesWithViewBag();
                ModelState.AddModelError("CategoryId", "This category does not exist");
                return View(product);
            }

            await _context.Products.AddAsync(product);
              await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAsync));
        }
        [HttpGet]
        public  async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
                return NotFound();

            await SendCategoriesWithViewBag();

            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update( Product product)
        {
            if (!ModelState.IsValid)
            {
                await SendCategoriesWithViewBag();
                return View(product);
            }
            var existProduct = await _context.Products.FindAsync(product.Id);
            if (existProduct is null)
                return BadRequest();
            var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == product.CategoryId);
            if (!isExistCategory)
            {
                await SendCategoriesWithViewBag();
                ModelState.AddModelError("CategoryId", "This category does not exist");
                return View(product);
            }
            existProduct.Name = product.Name;
            existProduct.Price = product.Price;
            existProduct.CategoryId = product.CategoryId;
            _context.Products.Update(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAsync));
        }
        private async Task SendCategoriesWithViewBag()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
        }
    }
}
