using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;

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
        [Authorize(Roles = AppRole.Admin)]
        public async Task<ActionResult> GetCustomers()
        {
            return Ok(await _repo.GetAll());
        }

        
        [HttpGet("get-by-id/{id}")]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<ActionResult> GetCustomer(string id)
        {
            return Ok(await _repo.GetByID(id));
        }


        [HttpPut("update-info/{customer}")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomer([FromForm] CustomerVM customer)
        {
            return Ok(await _repo.Update(customer));
        }

        
        [HttpPost("new-customer")]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> AddCustomer([FromForm] Customer customer)
        {
            return Ok(await _repo.Add(customer));
        }

        
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            return Ok(await _repo.Delete(id));
        }

        [HttpGet("search-by-name")]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> Search(string name)
        {
            return Ok(await _repo.SearchByName(name));
        }
    }
}
