using Company.Data.Entities;
using Company.Services.Dto;
using Company.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Web.Controllers
{
    [Authorize]

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationUser> _logger;

        public UserController(UserManager<ApplicationUser> userManager,
            ILogger<ApplicationUser> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string SearchInp)
        {
            List<ApplicationUser> users;

            if (string.IsNullOrEmpty(SearchInp))
                users = await _userManager.Users.ToListAsync();
            else
                users = await _userManager.Users
                    .Where(user => user.NormalizedEmail.Trim().Contains(SearchInp.Trim().ToUpper()))
                    .ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            if (ViewName == "Update")
            {
                var userViewModel = new UserUpdateViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName

                };
                return View(ViewName, userViewModel);
            }

            return View(ViewName, user);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, "Update");
        }


        [HttpPost]
        public async Task<IActionResult> Update(string id, UserUpdateViewModel applicationUser)
        {
            if (id != applicationUser.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);

                    if (user is null)
                        return NotFound();

                    user.UserName = applicationUser.UserName;
                    user.NormalizedUserName = applicationUser.UserName.ToUpper();

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User Updated Successfully");

                        return RedirectToAction("Index");
                    }

                    foreach (var item in result.Errors)
                        _logger.LogError(item.Description);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);

                }

            }
            return View(applicationUser);

        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user is null)
                    return NotFound();

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Index");


                foreach (var item in result.Errors)
                    _logger.LogError(item.Description);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return RedirectToAction("Index");

        }
    }
}
