using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.ViewModels;
using RetroFootballAPI.Repositories;

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


        [HttpGet("get-by-customer/{customerID}")]
        public async Task<IActionResult> GetByCustomer(string customerID)
        {
            return Ok(await _repo.GetByCustomer(customerID));
        }


        [HttpPost("add-to-wishlist")]
        public async Task<IActionResult> Add(WishListVM wishList)
        {
            return Ok(await _repo.Add(wishList));
        }


        [HttpDelete("remove/{customerID}/{productID}")]
        public async Task<IActionResult> Remove(string customerID, string productID)
        {
            return Ok(await _repo.Delete(customerID, productID));
        }
    }
}
