using Microsoft.AspNetCore.Mvc;
using Invoice_api.Manager;
using Invoice_api.Infraestructure.Dtos;

namespace Invoice_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerManager _customerManager;

        public CustomerController(CustomerManager customerManager)
        {
            _customerManager = customerManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerToSaveDto customerDto)
        {
            if (customerDto == null)
                return BadRequest(new { error = "El cliente es requerido." });

            var createdCustomer = await _customerManager.CreateCustomerAsync(customerDto);
            return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.CustomerId }, createdCustomer);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetCustomerById(long id)
        {
            var customer = await _customerManager.GetCustomerByIdAsync(id);
            return customer is not null
                ? Ok(customer)
                : NotFound(new { error = $"Cliente con ID {id} no encontrado." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerManager.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateCustomer(long id, [FromBody] CustomerToSaveDto customerDto)
        {
            if (customerDto == null)
                return BadRequest(new { error = "El cliente es requerido." });

            var updatedCustomer = await _customerManager.UpdateCustomerAsync(customerDto, id);
            return Ok(updatedCustomer);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            var deleted = await _customerManager.DeleteCustomerAsync(id);
            return deleted
                ? Ok(new { message = $"Cliente con ID {id} eliminado." })
                : NotFound(new { error = $"Cliente con ID {id} no encontrado." });
        }
    }
}
