using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _repo;

        public ProductsController(IProductRepo repo)
        {
            _repo = repo;
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }


        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            return Ok(await _repo.GetByID(id));
        }


        [HttpGet("best-seller")]
        public async Task<IActionResult> GetBestSeller()
        {
            return Ok(await _repo.BestSeller());
        }


        [HttpGet("new-arrival")]
        public async Task<IActionResult> GetNewArrivals()
        {
            return Ok(await _repo.NewArrivals());
        }


        [HttpPut("update/{product}")]
        public async Task<IActionResult> Update(Product product)
        {
            return Ok(await _repo.Update(product));
        }


        [HttpPost("new-product")]
        public async Task<IActionResult> Add(Product product)
        {
            return Ok(await _repo.Add(product));
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _repo.Delete(id));
        }
    }
}
