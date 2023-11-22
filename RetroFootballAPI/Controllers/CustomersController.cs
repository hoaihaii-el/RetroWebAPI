using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepo _repo;

        public CustomersController(ICustomerRepo repo)
        {
            _repo = repo;
        }


        [HttpGet("get-all")]
        public async Task<ActionResult> GetCustomers()
        {
            return Ok(await _repo.GetAll());
        }


        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult> GetCustomer(string id)
        {
            return Ok(await _repo.GetByID(id));
        }


        [HttpPut("update/{customer}")]
        public async Task<IActionResult> UpdateCustomer(Customer customer)
        {
            return Ok(await _repo.Update(customer));
        }


        [HttpPost("new-customer")]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            return Ok(await _repo.Add(customer));
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            return Ok(await _repo.Delete(id));
        }
    }
}
