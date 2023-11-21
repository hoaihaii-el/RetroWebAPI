using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.ViewModels;
using RetroFootballAPI.Repositories;

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



        [HttpGet("{customerID}")]
        public async Task<ActionResult> GetAll(string customerID)
        {
            return Ok(await _repo.GetAll(customerID));
        }


        [HttpGet("{customerID}/{priority}")]
        public async Task<ActionResult> GetDeliveryInfo(string customerID, int priority)
        {
            return Ok(await _repo.GetByID(customerID, priority));
        }


        [HttpPut("set-default/{customerID}/{priority}")]
        public async Task<IActionResult> SetDefault(string customerID, int priority)
        {
            return Ok(await _repo.SetDefault(customerID, priority));
        }


        [HttpPost]
        public async Task<ActionResult> Add(DeliveryInfoVM info)
        {
            return Ok(await _repo.Add(info));
        }


        [HttpPut("{info}")]
        public async Task<IActionResult> UpdateCustomer(DeliveryInfoVM info)
        {
            return Ok(await _repo.Update(info));
        }



        [HttpDelete("{customerID}/{priority}")]
        public async Task<IActionResult> Delete(string customerID, int priority)
        {
            return Ok(await _repo.Delete(customerID, priority));
        }
    }
}
