using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _repo;

        public OrdersController(IOrderRepo repo)
        {
            _repo = repo;
        }


        [HttpGet("get-today/{type}")]
        public async Task<IActionResult> GetToday(int type)
        {
            return Ok(await _repo.GetToday(type));
        }


        [HttpGet("get-by-month/{month}/{type}")]
        public async Task<IActionResult> GetByMonth(int month, int type)
        {
            return Ok(await _repo.GetByMonth(month, type));
        }


        [HttpGet("get-by-id/{customerID}/{type}")]
        public async Task<ActionResult> GetByCustomer(string customerID, int type)
        {
            return Ok(await _repo.GetByCustomer(customerID, type));
        }


        [HttpPost("new-order")]
        public async Task<IActionResult> Add(OrderVM order)
        {
            return Ok(await _repo.Add(order));
        }


        [HttpDelete("delete/{orderID}")]
        public async Task<IActionResult> Delete(int orderID)
        {
            return Ok(await _repo.Delete(orderID));
        }


        [HttpPut("update/{orderID}")]
        public async Task<IActionResult> UpdateStatus(int orderID)
        {
            return Ok(await _repo.UpdateStatus(orderID));
        }
    }
}
