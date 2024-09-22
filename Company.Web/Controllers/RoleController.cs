using Company.Data.Entities;
using Company.Services.Dto;
using Company.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Web.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IdentityRole> _logger;

        public RoleController(RoleManager<IdentityRole> roleManager ,
            UserManager<ApplicationUser> userManager
            , ILogger<IdentityRole> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = roleModel.Name
                };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));

                foreach (var item in result.Errors)
                    _logger.LogError(item.Description);
            }
            return View(roleModel);

        }
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();

            var roleViewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name

            };
            return View(ViewName, roleViewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, "Update");
        }


        [HttpPost]
        public async Task<IActionResult> Update(string id, RoleViewModel applicationRole)
        {
            if (id != applicationRole.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);

                    if (role is null)
                        return NotFound();

                    role.Name = applicationRole.Name;
                    role.NormalizedName = applicationRole.Name.ToUpper();

                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Role Updated Successfully");

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
            return View(applicationRole);

        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var Role = await _roleManager.FindByIdAsync(id);

                if (Role is null)
                    return NotFound();

                var result = await _roleManager.DeleteAsync(Role);

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

        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
                return NotFound();

            ViewBag.RoleId = roleId;

            var users = await _userManager.Users.ToListAsync();

            var usersInRole = new List<UserInRoleViewModel>();

            foreach (var user in users)
            {
                var userInRole = new UserInRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName

                };
                if (await _userManager.IsInRoleAsync(user , role.Name))
                    userInRole.IsSelected = true;
                else
                    userInRole.IsSelected = false;

                usersInRole.Add(userInRole);
            }

            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId , List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
                return NotFound();

            if(ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var AppUser = await _userManager.FindByIdAsync(user.UserId);

                    if (AppUser is not null)
                    {
                        if(user.IsSelected && !await _userManager.IsInRoleAsync(AppUser ,role.Name))
                            await _userManager.AddToRoleAsync(AppUser, role.Name);
                        else if(!user.IsSelected && await _userManager.IsInRoleAsync(AppUser, role.Name))
                            await _userManager.RemoveFromRoleAsync(AppUser, role.Name);
                    }
                }
                return RedirectToAction("Update" , new { id = roleId});
            }


            return View(users);
        }
    }
}
