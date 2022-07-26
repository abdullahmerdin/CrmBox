using Microsoft.AspNetCore.Mvc;

namespace CrmBox.WebUI.Controllers
{
    public class AdminsController : Controller
    {
        
        public IActionResult GetAll()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(int v)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Update(int adminId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int adminId)
        {
            return View();
        }

    }
}
