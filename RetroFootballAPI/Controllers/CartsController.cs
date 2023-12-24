using RetroFootballAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using RetroFootballAPI.StaticServices;

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
        [Authorize]
        public async Task<IActionResult> GetCarts(string customerID)
        {
            return Ok(await _repo.GetCarts(customerID));
        }


        [HttpGet("total/{customerID}")]
        [Authorize]
        public async Task<IActionResult> GetTotalCart(string customerID)
        {
            return Ok(await _repo.GetCartTotal(customerID));
        }


        [HttpGet("get-checkout-info/{customerID}")]
        [Authorize]
        public async Task<IActionResult> GetCheckoutInfo(string customerID)
        {
            return Ok(await _repo.GetCheckoutInfo(customerID));
        }

        [HttpGet("items/{customerID}")]
        [Authorize]
        public async Task<IActionResult> GetCartItems(string customerID)
        {
            return Ok(await _repo.GetTotalItems(customerID));
        }


        [HttpPost("add-to-cart")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromForm] CartVM cart)
        {
            if (cart.Quantity < 1 || !ProductSize.Sizes.Contains(cart.Size ?? ""))
            {
                return BadRequest();
            }

            return Ok(await _repo.AddToCart(cart));
        }


        [HttpPut("increase/{customerID}/{productID}/{size}")]
        [Authorize]
        public async Task<IActionResult> IncreaseQuantity(string customerID, string productID, string size)
        {
            return Ok(await _repo.IncreaseQuantity(customerID, productID, size));
        }


        [HttpPut("decrease/{customerID}/{productID}/{size}")]
        [Authorize]
        public async Task<IActionResult> DecreaseQuantity(string customerID, string productID, string size)
        {
            return Ok(await _repo.DecreaseQuantity(customerID, productID, size));
        }


        [HttpDelete("remove/{customerID}/{productID}/{size}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(string customerID, string productID, string size)
        {
            return Ok(await _repo.RemoveFromCart(customerID, productID, size));
        }


        [HttpDelete("clear/{customerID}")]
        [Authorize]
        public async Task<IActionResult> ClearCart(string customerID)
        {
            await _repo.ClearCart(customerID);
            return Ok(
                new { success = true }
            );
        }
    }
}
