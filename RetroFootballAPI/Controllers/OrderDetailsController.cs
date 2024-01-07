using RetroFootballAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailRepo _repo;

        public OrderDetailsController(IOrderDetailRepo repo)
        {
            _repo = repo;
        }


        [HttpGet("get-detail/{orderID}")]
        [Authorize]
        public async Task<IActionResult> GetByOrderID(int orderID)
        {
            return Ok(await _repo.GetByOrderID(orderID));
        }


        [HttpPost("add-detail")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] OrderDetailVM orderDetail)
        {
            return Ok(await _repo.Add(orderDetail));
        }


        [HttpDelete("delete/{customerID}/{productID}/{size}")]
        [Authorize]
        public async Task<IActionResult> Delete(string customerID, string productID, string size)
        {
            return Ok(await _repo.Delete(customerID, productID, size));
        }
    }
}
