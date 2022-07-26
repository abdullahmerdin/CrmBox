using CrmBox.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CrmBox.Core.Domain;

namespace OnionCRM.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PhoneNumbersController : ControllerBase
    {

        private readonly ICustomerService _customerService;

        public PhoneNumbersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNumbers(ICustomerService _phoneNumberService)
        {
            var numbers = await _phoneNumberService.GetAllCustomers();
            return Ok(numbers);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoneNumber(Customer customer)
        {
            var number = await _customerService.AddCustomer(customer);
            return Ok(number);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            return Ok(await _customerService.GetCustomerById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Customer customer)
        {

            return Ok(await _customerService.UpdateCustomer(customer));

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteCustomer(id);
            return Ok();
        }

    }

}



