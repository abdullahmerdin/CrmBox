using CrmBox.Core.Domain.Identity;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrmBox.WebUI.Controllers
{
    public class AppRolesController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;

        public AppRolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRoleVM model)
        {
            if (ModelState.IsValid)
            {
                AppRole role = new AppRole
                {
                    Name = model.Name
                };

                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("GetAll");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var values = _roleManager.Roles.FirstOrDefault(x => x.Id == id);

            UpdateRoleVM model = new UpdateRoleVM
            {
                Id = values.Id,
                Name = values.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRoleVM model)
        {
            var values = _roleManager.Roles.Where(x => x.Id == model.Id).FirstOrDefault();

            values.Name = model.Name;

            var result = await _roleManager.UpdateAsync(values);

            if (result.Succeeded)
            {
                return RedirectToAction("GetAll");
            }
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            var values = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
            var result = await _roleManager.DeleteAsync(values);
            if (result.Succeeded)
            {
                return RedirectToAction("GetAll");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(int userId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            var roles = _roleManager.Roles.ToList();

            TempData["UserId"] = user.Id;

            var userRoles = await _userManager.GetRolesAsync(user);

            List<AssignRoleVM> model = new List<AssignRoleVM>();
            foreach (var item in roles)
            {
                AssignRoleVM m = new AssignRoleVM();
                m.RoleId = item.Id;
                m.Name = item.Name;
                m.Exist = userRoles.Contains(item.Name);
                model.Add(m);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(List<AssignRoleVM> model)
        {
            var userId = (int)TempData["UserId"];
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            foreach (var item in model)
            {
                if (item.Exist)
                {
                    await _userManager.AddToRoleAsync(user, item.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.Name);
                }
            }
            return RedirectToAction("GetAll");
        }
    }
}
