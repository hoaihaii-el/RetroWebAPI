using RetroFootballAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Repositories;

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


        [HttpGet("{orderID}")]
        public async Task<IActionResult> GetByOrderID(int orderID)
        {
            return Ok(await _repo.GetByOrderID(orderID));
        }


        [HttpPost]
        public async Task<IActionResult> Add(OrderDetailVM orderDetail)
        {
            return Ok(await _repo.Add(orderDetail));
        }


        [HttpDelete("{customerID}/{productID}/{size}")]
        public async Task<IActionResult> Delete(string customerID, string productID, string size)
        {
            return Ok(await _repo.Delete(customerID, productID, size));
        }
    }
}
