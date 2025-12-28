using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Intro.Contexts;
using MVC_Intro.Models;

namespace MVC_Intro.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class SliderController(AppDbContext _context) : Controller
    {
        //private readonly AppDbContext _context;

        //public SliderController(AppDbContext context)
        //{
        //    _context = context;
        //}

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

            //if(!ModelState.IsValid)
            // {
            //     return View();
            // }

            //var isExist= await _context.Sliders.AnyAsync(x=>x.Title==slider.Title);
            //   if(isExist)
            //     {
            //      ModelState.AddModelError("Title", "This title already exist");
            //      return View();
            // }



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
