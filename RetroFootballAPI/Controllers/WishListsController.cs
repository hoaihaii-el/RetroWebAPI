using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.ViewModels;
using RetroFootballAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class WishListsController : ControllerBase
    {
        private readonly IWishListRepo _repo;

        public WishListsController(IWishListRepo repo)
        {
            _repo = repo;
        }

        [Authorize]
        [HttpGet("get-by-customer/{customerID}")]
        public async Task<IActionResult> GetByCustomer(string customerID)
        {
            return Ok(await _repo.GetByCustomer(customerID));
        }

        [Authorize]
        [HttpPost("add-to-wishlist")]
        public async Task<IActionResult> Add([FromForm] WishListVM wishList)
        {
            return Ok(await _repo.Add(wishList));
        }

        [Authorize]
        [HttpDelete("remove/{customerID}/{productID}")]
        public async Task<IActionResult> Remove(string customerID, string productID)
        {
            return Ok(await _repo.Delete(customerID, productID));
        }
    }
}
