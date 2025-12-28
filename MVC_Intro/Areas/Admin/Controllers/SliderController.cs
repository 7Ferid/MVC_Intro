using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using MVC_Intro.Contexts;
using MVC_Intro.Models;

namespace MVC_Intro.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class SliderController(AppDbContext _context, IWebHostEnvironment enviroment) : Controller
    {
        //private readonly AppDbContext _context;

        //public SliderController(AppDbContext context)
        //{
        //    _context = context;
        //}
        private readonly IWebHostEnvironment _enviroment;

        public async Task<IActionResult> IndexAsync()
        {
            var sliders = await _context.Sliders.ToListAsync();
            return View(sliders);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (slider.Logo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Logo", "File type must be image");
                return View(slider);
            }
            if (slider.Logo.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Logo", "Image size must be max 2MB");
                return View(slider);

            }

            //var isExist= await _context.Sliders.AnyAsync(x=>x.Title==slider.Title);
            //   if(isExist)
            //     {
            //      ModelState.AddModelError("Title", "This title already exist");
            //      return View();
            // }
            string uniqueFileName = Guid.NewGuid().ToString() + slider.Logo.FileName;

            string imageFolderPath = $@"{_enviroment.WebRootPath}\assets\images\website-images\{uniqueFileName}";
            using FileStream stream = new(imageFolderPath, FileMode.Create);
            await slider.Logo.CopyToAsync(stream);
            slider.LogoUrl = uniqueFileName;



            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider is null)
                return NotFound();
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var slider = _context.Sliders.FindAsync(id);
            if (slider is { })
                return NotFound();
            return View(slider);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Slider slider)
        {
            if (!ModelState.IsValid)
                return View();
            var existSlider = await _context.Sliders.FindAsync(slider.Id);
            if (existSlider is null)
                return BadRequest();
            existSlider.Title = slider.Title;
            existSlider.Description = slider.Description;
            existSlider.LogoUrl = slider.LogoUrl;
            _context.Sliders.Update(existSlider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
