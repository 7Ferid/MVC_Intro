using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Construction;
using Microsoft.EntityFrameworkCore;
using MVC_Intro.Contexts;
using MVC_Intro.Models;
using MVC_Intro.ViewModels.ProductViewModels;
using System.Threading.Tasks;

namespace MVC_Intro.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(AppDbContext _context,IWebHostEnvironment _envoriement) : Controller
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
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {

          
            if (!ModelState.IsValid)
            {  await SendCategoriesWithViewBag();
                return View(vm);
            }
            var isExistCategory= await _context.Categories.AnyAsync(x=>x.Id==vm.CategoryId);

            if(!isExistCategory)
            {
                await SendCategoriesWithViewBag();
                ModelState.AddModelError("CategoryId", "This category does not exist");
                return View(vm);
            }

            if (!vm.MainImage.ContentType.Contains("image")){
                ModelState.AddModelError("MainImage", "File type must be image");
                return View(vm);
            }
            if (vm.MainImage.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("MainImage", "Image size must be max 2MB");
                return View(vm);
            }
            if (!vm.HoverImage.ContentType.Contains("image"))
            {
                ModelState.AddModelError("HoverImage", "File type must be image");
                return View(vm);
            }
            if (vm.HoverImage.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("HoverImage", "Image size must be max 2MB");
                return View(vm);
            }
            string uniqueMainFileName = Guid.NewGuid().ToString() + vm.MainImage.FileName;
            string mainImagePath =Path.Combine( _envoriement.WebRootPath, "assets", "images", "website-images", uniqueMainFileName);

            using FileStream mainStream = new FileStream(mainImagePath, FileMode.Create);
           await  vm.MainImage.CopyToAsync(mainStream);


            string uniqueHoverFileName = Guid.NewGuid().ToString() + vm.HoverImage.FileName;
            string hoverImagePath = Path.Combine(_envoriement.WebRootPath, "assets", "images", "website-images", uniqueHoverFileName);

            using FileStream hoverStream = new FileStream(hoverImagePath, FileMode.Create);
            await vm.HoverImage.CopyToAsync(hoverStream);


            Product product = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                MainImagePath = uniqueMainFileName,
                HoverImagePath= uniqueHoverFileName,
                ReytingCount = vm.ReytingCount

            };


            await _context.Products.AddAsync(product);
              await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
                return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            string folderpath=Path.Combine(_envoriement.WebRootPath, "assets", "images", "website-images");
            string mainImagePath = Path.Combine(folderpath, product.MainImagePath);
            string hoverImagePath = Path.Combine(folderpath, product.HoverImagePath);
            if (System.IO.File.Exists(mainImagePath))
            {
                System.IO.File.Delete(mainImagePath);
            }
            if (System.IO.File.Exists(hoverImagePath))
            {
                System.IO.File.Delete(hoverImagePath);
            }


            return RedirectToAction("Index");
        }

        private async Task SendCategoriesWithViewBag()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
        }
    }
}
