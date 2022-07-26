using CrmBox.Core.Domain.Identity;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrmBox.WebUI.Controllers
{
    public class AppUsersController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;

        public AppUsersController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Roles = _roleManager.Roles.Select(x => new RoleWithSelectVM
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = false,
                Roles = _roleManager.Roles.ToList()
            });


            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserVM model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                {
                    foreach (var role in model.Roles.Where(x => x.IsSelected = true))
                    {
                        await _userManager.AddToRoleAsync(appUser, role.Name);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(int userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.DeleteAsync(user);
            return RedirectToAction("GetAll");
        }
    }
}
