using RetroFootballAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Repositories;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepo _repo;

        public CartsController(ICartRepo repo)
        {
            _repo = repo;
        }


        
        [HttpGet("{customerID}")]
        public async Task<IActionResult> GetCarts(string customerID)
        {
            return Ok (await _repo.GetCarts(customerID));
        }


        [HttpGet("total/{customerID}")]
        public async Task<IActionResult> GetTotalCart(string customerID)
        {
            return Ok(await _repo.GetCartTotal(customerID));
        }


        [HttpGet("items/{customerID}")]
        public async Task<IActionResult> GetCartItems(string customerID)
        {
            return Ok(await _repo.GetTotalItems(customerID));
        }


        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart(CartVM cart)
        {
            try
            {
                return Ok(await _repo.AddToCart(cart));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPut("increase/{customerID}/{productID}/{size}")]
        public async Task<IActionResult> IncreaseQuantity(string customerID, string productID, string size)
        {
            return Ok(await _repo.IncreaseQuantity(customerID, productID, size));
        }


        [HttpPut("decrease/{customerID}/{productID}/{size}")]
        public async Task<IActionResult> DecreaseQuantity(string customerID, string productID, string size)
        {
            return Ok(await _repo.DecreaseQuantity(customerID, productID, size));
        }


        [HttpDelete("remove/{customerID}/{productID}/{size}")]
        public async Task<IActionResult> RemoveFromCart(string customerID, string productID, string size)
        {
            return Ok(await _repo.RemoveFromCart(customerID, productID, size));
        }


        [HttpDelete("clear/{customerID}")]
        public async Task<IActionResult> ClearCart(string customerID)
        {
            await _repo.ClearCart(customerID);
            return Ok(new { success = true });
        }
    }
}
