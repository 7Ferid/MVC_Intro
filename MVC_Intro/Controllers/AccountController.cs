using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVC_Intro.Abstraction;
using MVC_Intro.Contexts;
using MVC_Intro.ViewModels.UserViewModels;
using System.Threading.Tasks;

namespace MVC_Intro.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager,SignInManager<AppUser>_signInManager,RoleManager<IdentityRole> _rolemanager,IEmailService _emailService ):Controller
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

           

          await  SendConfirmationMailAsync(newUser);

            TempData["SuccessMessage"] = "Registardan ugurla kecdiniz zehmet olmasa emailinizi tesdiqleyin ";
         


            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            TempData["ErrorMessage"] = "xeta bas verdi";

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

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Please confirm your email");
                await SendConfirmationMailAsync(user);
                return View(vm);
            }



            await _signInManager.SignInAsync(user, vm.IsRemember);


            if (!string.IsNullOrWhiteSpace(vm.ReturnUrl))
                return Redirect(vm.ReturnUrl);


            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        private async Task SendConfirmationMailAsync(AppUser user)
        {
           string token= await _userManager.GenerateEmailConfirmationTokenAsync(user);
          /*  string url = $"https://localhost:7181/Account/ConfirmEmail?token{token}&userId={user.Id}";
*/
          var url=Url.Action("ConfirmEmail","Account",new { token=token ,userId=user.Id},Request.Scheme);
            string emailBody = $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Confirmation Email</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f7;
            margin: 0;
            padding: 0;
        }}
        .container {{
            width: 100%;
            padding: 20px;
            background-color: #f4f4f7;
        }}
        .email-content {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }}
        .header {{
            background-color: #4CAF50;
            color: white;
            text-align: center;
            padding: 30px 20px;
        }}
        .header h1 {{
            margin: 0;
            font-size: 24px;
        }}
        .body {{
            padding: 30px 20px;
            color: #333333;
        }}
        .body p {{
            font-size: 16px;
            line-height: 1.6;
        }}
        .button {{
            display: inline-block;
            margin-top: 20px;
            padding: 12px 25px;
            font-size: 16px;
            color: white;
            background-color: #4CAF50;
            text-decoration: none;
            border-radius: 5px;
        }}
        .footer {{
            text-align: center;
            font-size: 12px;
            color: #999999;
            padding: 20px;
        }}
        @media (max-width: 600px) {{
            .body, .header {{
                padding: 20px;
            }}
            .button {{
                padding: 10px 20px;
            }}
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""email-content"">
            <div class=""header"">
                <h1>Confirm Your Email</h1>
            </div>
            <div class=""body"">
                <p>Hi <strong>{user.UserName}</strong>,</p>
                <p>Thank you for signing up! Please confirm your email address by clicking the button below:</p>
                <a href=""{url}"" class=""button"">Confirm Email</a>
                <p>If the button doesn’t work, copy and paste the following link into your browser:</p>
                <p><a href=""{url}"">{url}</a></p>
                <p>Welcome aboard!</p>
                <p>— The Team</p>
            </div>
            <div class=""footer"">
                <p>&copy; 2026 Ferid. All rights reserved.</p>
            </div>
        </div>
    </div>
</body>
</html>
";



            await _emailService.SendEmailAsync(user.Email!, "confirm your email", emailBody);


        }
        public async Task<IActionResult> ConfirmEmail(string token,string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user is null)
                return NotFound();


            var result = await _userManager.ConfirmEmailAsync(user, token);


            if (!result.Succeeded)
            {
                return BadRequest();

            }
            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index","Home");
        }

     /*   public async Task<IActionResult> CreateRoles()
        {
            await _rolemanager.CreateAsync(new IdentityRole()
            {
                Name = "User"
            });
            await _rolemanager.CreateAsync(new IdentityRole()
            {
                Name = "Admin"
            });
            await _rolemanager.CreateAsync(new IdentityRole()
            {
                Name = "Moderator"
            });
            return Ok("Roles created");



        }*/

    }
}
