using CrmBox.Core.Domain.Identity;
using CrmBox.Infrastructure.Extensions.CustomClaimType;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrmBox.WebUI.Controllers
{
    public class AppRolesController : AuthController
    {
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;

        public AppRolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : base(userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Policy = "GetAllUserRoles")]
        public IActionResult GetAllUserRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        [Authorize(Policy = "AddUserRole")]
        public IActionResult AddUserRole()
        {
            AddRoleVM model = new();
            model.Policies =  Infrastructure.Extensions.Policies.PolicyTypes.Policies.Select(x => new PolicyWithIsSelectedVM { Policy = x, IsSelected = false }).ToList();
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "AddUserRole")]
        public async Task<IActionResult> AddUserRole(AddRoleVM model)
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
                    foreach (var policy in model.Policies.Where(x => x.IsSelected))
                    {
                        await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, policy.Policy));
                    }
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
        [Authorize(Policy = "UpdateUserRole")]
        public IActionResult UpdateUserRole(int id)
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
        [Authorize(Policy = "UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(UpdateRoleVM model)
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

        [Authorize(Policy = "DeleteUserRole")]
        public async Task<IActionResult> DeleteUserRole(int id)
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
        [Authorize(Policy = "AssignUserRole")]
        public async Task<IActionResult> AssignUserRole(int userId)
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
        [Authorize(Policy = "AssignUserRole")]
        public async Task<IActionResult> AssignUserRole(List<AssignRoleVM> model)
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
            return RedirectToAction("UserRoleList");
        }
    }
}
