using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Intro.Contexts;
using MVC_Intro.Models;

namespace MVC_Intro.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController(AppDbContext _context) : Controller
    {
        //private readonly AppDbContext _context;

        //public SliderController(AppDbContext context)
        //{
        //    _context = context;
        //}

        public async Task<IActionResult> IndexAsync()
        {
            var sliders= await _context.Sliders.ToListAsync();
            return View(sliders);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {

           if(!ModelState.IsValid)
            {
                return View();
            }

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
    }
}
