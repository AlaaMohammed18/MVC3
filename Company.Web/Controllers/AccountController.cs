using Company.Data.Entities;
using Company.Services.Helper;
using Company.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel input)
        {
            if (ModelState.IsValid)
            {
                var User = new ApplicationUser
                {
                    UserName = input.Email.Split("@")[0],
                    Email = input.Email,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(User, input.Password);

                if (result.Succeeded)
                    return RedirectToAction("Login");

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(input);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(input.Email);

                if (user is not null)
                {
                    if (await _userManager.CheckPasswordAsync(user, input.Password))
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, input.Password, input.RememberMe, true);

                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Incorrect Email Or Password");

                return View(input);
            }

            return View(input);

        }

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public new async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(input.Email);

                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var URL = Url.Action("ResetPassword", "Account", new { Email = input.Email, Token = token }, Request.Scheme);

                    var email = new Email
                    {
                        Body = URL,
                        Subject = "Reset Password",
                        To = input.Email

                    };

                    EmailSettings.SendEmail(email);

                    return RedirectToAction(nameof(CheckYourInbox));
                }
            }


            return View(input);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }
        public IActionResult ResetPassword(string Email , string Token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(input.Email);

                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user,input.Token , input.Password);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(Login));

                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }

            }
            return View(input);
        }



    }
}
