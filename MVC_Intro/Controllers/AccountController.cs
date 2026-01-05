using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Intro.Contexts;
using MVC_Intro.ViewModels.UserViewModels;
using System.Threading.Tasks;

namespace MVC_Intro.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager,SignInManager<AppUser>_signInManager):Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);


            var existUser = await _userManager.FindByNameAsync(vm.UserName);
            if(existUser is { })
            {
                ModelState.AddModelError("Username", "This uername is already exist");
                return View(vm);
            }
            existUser = await _userManager.FindByEmailAsync(vm.EmailAdress);
            if(existUser is { })
            {
                ModelState.AddModelError(nameof(vm.EmailAdress), "This email is already exist");
                return View(vm);
            }

            AppUser newUser = new() { 
            FullName=vm.FirstName+" "+vm.LastName,
            Email=vm.EmailAdress,
            UserName=vm.UserName
            };

         var result=  await  _userManager.CreateAsync(newUser, vm.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(vm);
            }


            return Ok("ok");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            var user = await _userManager.FindByEmailAsync(vm.EmailAdress);

            if (user is null)
            {
                ModelState.AddModelError("", "Email or password is wrong ");
                return View(vm);

            }

            var loginResult=await _userManager.CheckPasswordAsync(user, vm.Password);
            if (!loginResult)
            {
                ModelState.AddModelError("", "Email or password is wrong ");
                return View(vm);
            }

            await _signInManager.SignInAsync(user, false);

            return Ok($"{user.FullName} Welcome");

        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

    }
}
