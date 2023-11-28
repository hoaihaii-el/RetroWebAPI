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

        [HttpGet("top-selling/{month}/{year}")]
        public async Task<IActionResult> TopSelling(int month, int year)
        {
            return Ok(await _repo.TopSelling(month, year));
        }

        [HttpGet("get-by-cate/{cate}/{value}/{page}/{productPerPage}")]
        public async Task<IActionResult> GetByCate(string cate, string value, int page, int productPerPage)
        {
            return Ok(await _repo.GetByCategory(cate, value, page, productPerPage));
        }

        [HttpGet("get-by-price/{min}/{max}/{page}/{productPerPage}")]
        public async Task<IActionResult> GetByPrice(decimal min, decimal max, int page, int productPerPage)
        {
            return Ok(await _repo.GetByPrice(min, max, page, productPerPage));
        }

        [HttpGet("get-all-by-page/{cate}/{value}/{page}/{productPerPage}")]
        public async Task<IActionResult> GetAllByPage(int page, int productPerPage)
        {
            return Ok(await _repo.GetProductByPage(page, productPerPage));
        }

        [HttpGet("get-by-checkbox/{value}/{page}/{productPerPage}")]
        public async Task<IActionResult> GetByCheckBox(List<string> value, int page, int productPerPage)
        {
            return Ok(await _repo.GetByCheckBox(value, page, productPerPage));
        }

        [HttpGet("get-by-search/{value}/{page}/{productPerPage}")]
        public async Task<IActionResult> GetBySearch(string value, int page, int productPerPage)
        {
            return Ok(await _repo.GetBySearch(value, page, productPerPage));
        }
    }
}
