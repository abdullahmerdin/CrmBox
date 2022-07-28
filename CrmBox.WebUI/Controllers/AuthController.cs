using CrmBox.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrmBox.WebUI.Controllers
{
    public class AuthController : Controller
    {
        readonly UserManager<AppUser> _userManager;

        public AuthController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public AppUser user => _userManager.GetUserAsync(User).Result;
        public int Id => user.Id;
        public string FirstName => user.FirstName;
        public string LastName => user.LastName;
        public string UserName => user.UserName;
    }
}
