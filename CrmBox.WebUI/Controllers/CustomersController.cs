using CrmBox.Application.Interfaces;
using CrmBox.Core.Domain;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrmBox.WebUI.Controllers
{
    public class CustomersController : Controller
    {
        readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
     
        public IActionResult GetAll()
        {
            var result = _customerService.GetAll();
            return View(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCustomerVM model)
        {
            if (ModelState.IsValid)
            {
                await _customerService.AddAsync(new()
                {
                    Address = model.Address,
                    CompanyName = model.CompanyName,
                    CreatedTime = DateTime.Now,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    JobTitle = model.JobTitle,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                });
                return RedirectToAction("GetAll");

            }
            throw new Exception("Ekleme işlemi esnasında bir hata meydana geldi");
        }

        [HttpGet]
       
        public IActionResult Update(int id)
        {
            Customer? customer = _customerService.Get(x => x.Id == id);
            if (customer != null)
                return View(customer);
            else
                throw new Exception("Belirtilen id ile eşleşen bir müşteri bulunamadı.");
        }

        [HttpPost]
        public IActionResult Update(Customer model)
        {
            if (ModelState.IsValid)
            {
                _customerService.Update(model);
                return RedirectToAction("GetAll");
            }
            throw new Exception("Güncelleme işlemi esnasında bir hata meydana geldi");
        }

        [HttpGet]
     
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.DeleteAsync(id);
            return RedirectToAction("GetAll");
        }
    }
}
