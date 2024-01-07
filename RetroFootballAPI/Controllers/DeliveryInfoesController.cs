using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.ViewModels;
using RetroFootballAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class DeliveryInfoesController : ControllerBase
    {
        private readonly IDeliveryInfoRepo _repo;

        public DeliveryInfoesController(IDeliveryInfoRepo repo)
        {
            _repo = repo;
        }


        [HttpGet("get-all/{customerID}")]
        [Authorize]
        public async Task<ActionResult> GetAll(string customerID)
        {
            return Ok(await _repo.GetAll(customerID));
        }


        [HttpGet("get-by-id/{customerID}/{priority}")]
        [Authorize]
        public async Task<ActionResult> GetDeliveryInfo(string customerID, int priority)
        {
            return Ok(await _repo.GetByID(customerID, priority));
        }


        [HttpPut("set-default/{customerID}/{priority}")]
        [Authorize]
        public async Task<IActionResult> SetDefault(string customerID, int priority)
        {
            return Ok(await _repo.SetDefault(customerID, priority));
        }


        [HttpPost("new-info")]
        [Authorize]
        public async Task<ActionResult> Add([FromBody] DeliveryInfoVM info)
        {
            return Ok(await _repo.Add(info));
        }


        [HttpPut("update/{pri}")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomer([FromBody] DeliveryInfoVM info, int pri)
        {
            return Ok(await _repo.Update(info, pri));
        }


        [HttpDelete("delete/{customerID}/{priority}")]
        [Authorize]
        public async Task<IActionResult> Delete(string customerID, int priority)
        {
            return Ok(await _repo.Delete(customerID, priority));
        }
    }
}
