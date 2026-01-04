using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Construction;
using Microsoft.EntityFrameworkCore;
using MVC_Intro.Contexts;
using MVC_Intro.Helpers;
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
            List<ProductGetVM> vm = await _context.Products.Include(x => x.Category)
                .Select(product=>new ProductGetVM()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryName = product.Category.Name,
                    MainImagePath = product.MainImagePath,
                    HoverImagePath = product.HoverImagePath
                }).ToListAsync();
           /* List<ProductGetVM> vms = new();
            foreach (var product in products)
            {
                ProductGetVM vm = new() { 
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryName = product.Category.Name,
                    MainImagePath = product.MainImagePath,
                    HoverImagePath = product.HoverImagePath

                };
                vms.Add(vm);

            }*/
            return View(vm);
        }
        public async Task<IActionResult> Create()
        {
            await SendItemsWithViewBag();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {

         
            if (!ModelState.IsValid)
            {   await SendItemsWithViewBag();
                return View(vm);
            }
            var isExistCategory= await _context.Categories.AnyAsync(x=>x.Id==vm.CategoryId);

            if(!isExistCategory)
            {
                await SendItemsWithViewBag();
                ModelState.AddModelError("CategoryId", "This category does not exist");
                return View(vm);
            }
            foreach(var tagId in vm.TagIds)
            {
                var isExistTag = await _context.Tags.AnyAsync(x => x.Id == tagId);
                if (!isExistTag)
                {
                    await SendItemsWithViewBag();
                    ModelState.AddModelError("TagIds", $"Tag with Id {tagId} does not exist");
                    return View(vm);
                }
            }

            if (!vm.MainImage.CheckType()){
                ModelState.AddModelError("MainImage", "File type must be image");
                return View(vm);
            }
            if (vm.MainImage.CheckSize(2))
            {
                ModelState.AddModelError("MainImage", "Image size must be max 2MB");
                return View(vm);
            }
            if (!vm.HoverImage.CheckType())
            {
                ModelState.AddModelError("HoverImage", "File type must be image");
                return View(vm);
            }
            if (vm.HoverImage.CheckSize(2))
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
                ReytingCount = vm.ReytingCount,
                ProductTags = []

            };
            foreach(var tagId in vm.TagIds)
            {
                ProductTag productTag = new()
                {
                    TagId = tagId,
                    Product = product
                };
                product.ProductTags.Add(productTag);
            }


            await _context.Products.AddAsync(product);
              await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public  async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.Include(x=>x.ProductTags).FirstOrDefaultAsync(x=>x.Id==id);
            if (product is null)
                return NotFound();

            await SendItemsWithViewBag();

            ProductUpdateVM vm=new ProductUpdateVM()
            {
                Id=product.Id,
                Name=product.Name,
                Description=product.Description,
                Price=product.Price,
                CategoryId=product.CategoryId,
                ReytingCount=product.ReytingCount,
                MainImagePath=product.MainImagePath,
                HoverImagePath=product.HoverImagePath,
                TagIds=product.ProductTags.Select(x=>x.TagId).ToList()
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update( ProductUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await SendItemsWithViewBag();
                return View(vm);
            }
            foreach (var tagId in vm.TagIds)
            {
                var isExistTag = await _context.Tags.AnyAsync(x => x.Id == tagId);
                if (!isExistTag)
                {
                    await SendItemsWithViewBag();
                    ModelState.AddModelError("TagIds", $"Tag with Id {tagId} does not exist");
                    return View(vm);
                }
            }



            if (!vm.MainImage?.CheckType() ?? false)
            {
                ModelState.AddModelError("MainImage", "File type must be image");
                return View(vm);
            }
            if (vm.MainImage?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("MainImage", "Image size must be max 2MB");
                return View(vm);
            }
            if (!vm.HoverImage?.CheckType() ?? false)
            {
                ModelState.AddModelError("HoverImage", "File type must be image");
                return View(vm);
            }
            if (vm.HoverImage?.CheckSize(2) ?? false)
            {
                ModelState.AddModelError("HoverImage", "Image size must be max 2MB");
                return View(vm);
            }


            var existProduct = await _context.Products.Include(x=>x.ProductTags).FirstOrDefaultAsync(x=>x.Id==vm.Id);
            if (existProduct is null)
                return BadRequest();
            var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!isExistCategory)
            {
                await SendItemsWithViewBag();
                ModelState.AddModelError("CategoryId", "This category does not exist");
                return View(vm);
            }
            existProduct.Name = vm.Name;
            existProduct.Description = vm.Description;
            existProduct.Price = vm.Price;
            existProduct.CategoryId = vm.CategoryId;
            existProduct.ProductTags = [];
            foreach(var tagId in vm.TagIds)
            {
                ProductTag productTag = new()
                {
                    TagId = tagId,
                    ProductId = existProduct.Id
                };
                existProduct.ProductTags.Add(productTag);
            }


            string folderPath = Path.Combine(_envoriement.WebRootPath, "assets", "images", "website-images");

            if (vm.MainImage is { })
            {
                string newMainImagePath = await vm.MainImage.SaveFileAsync(folderPath);
                string existingMainImagePath = Path.Combine(folderPath, existProduct.MainImagePath);
                ExtensionMethods.DeleteFile(existingMainImagePath);
                existProduct.MainImagePath = newMainImagePath;

            }
            if (vm.HoverImage is { })
            {
                string newHoverImagePath = await vm.HoverImage.SaveFileAsync(folderPath);
                string existingHoverImagePath = Path.Combine(folderPath, existProduct.HoverImagePath);
                ExtensionMethods.DeleteFile(existingHoverImagePath);
                existProduct.MainImagePath = newHoverImagePath;

            }




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

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products.Include(x=>x.Category).Select(product=>   new ProductGetVM()
            {
             
                
                    Id=product.Id,
                    Name=product.Name,
                    Description=product.Description,
                    Price=product.Price,
                    CategoryName=product.Category.Name,
                    MainImagePath=product.MainImagePath,
                    HoverImagePath=product.HoverImagePath,
                    TagNames=product.ProductTags.Select(x=>x.Tag.Name).ToList()


            }).FirstOrDefaultAsync(x=>x.Id==id);
            if (product is null)
                return NotFound();
            return View(product);
        }

        private async Task SendItemsWithViewBag()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            var tags= await _context.Tags.ToListAsync();
            ViewBag.Tags = tags;
        }
    }
}
