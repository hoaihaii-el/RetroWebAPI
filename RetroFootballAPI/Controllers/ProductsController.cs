using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;

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
            try
            {
                return Ok(await _repo.BestSeller());
            }
            catch
            {
                return NotFound();
            }
        }


        [HttpGet("new-arrival")]
        public async Task<IActionResult> GetNewArrivals()
        {
            return Ok(await _repo.NewArrivals());
        }


        [HttpPut("update/{productID}")]
        public async Task<IActionResult> Update(string productID, ProductVM2 product)
        {
            return Ok(await _repo.Update(productID, product));
        }

        
        [HttpPost("new-product")]
        //[Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> Add([FromBody] ProductVM product)
        {
            return Ok(await _repo.Add(product));
        }

        
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _repo.Delete(id));
        }


        [HttpGet("top-selling/{month}/{year}")]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> TopSelling(int month, int year)
        {
            return Ok(await _repo.TopSelling(month, year));
        }

        [HttpGet("get-by-cate/{cate}/{value}/{page}/{productPerPage}")]
        public async Task<IActionResult> GetByCate(string cate, string value, int page, int productPerPage)
        {
            return Ok(await _repo.GetByCategory(cate, value, page, productPerPage));
        }

        [HttpGet("filter-by")]
        public async Task<IActionResult> FilterBy(
            [FromQuery] List<string> names,
            [FromQuery] List<string> seasons,
            [FromQuery] List<string> groups,
            [FromQuery] bool club = true,
            [FromQuery] bool nation = true,
            [FromQuery] decimal minPrice = 0,
            [FromQuery] decimal maxPrice = 10000000,
            [FromQuery] string sortBy = "Name",
            [FromQuery] bool descending = false,
            [FromQuery] bool sizeS = false,
            [FromQuery] bool sizeM = false,
            [FromQuery] bool sizeL = false,
            [FromQuery] bool sizeXL = false,
            [FromQuery] int page = 1,
            [FromQuery] int productPerPage = 8)
        {
            return Ok(
                await _repo.FilterBy(
                    names,
                    seasons,
                    groups,
                    club,
                    nation,
                    minPrice,
                    maxPrice,
                    sortBy,
                    descending,
                    sizeS,
                    sizeM,
                    sizeL,
                    sizeXL,
                    page, 
                    productPerPage)
                );
        }

        [HttpGet("get-by-groups")]
        public async Task<IActionResult> GetByLeague([FromQuery] List<string> groups)
        {
            return Ok(await _repo.GetByGroup(groups));
        }

        [HttpGet("get-by-search/{value}/{page}/{productPerPage}")]
        public async Task<IActionResult> GetBySearch(string value, int page, int productPerPage)
        {
            return Ok(await _repo.GetBySearch(value, page, productPerPage));
        }

        [HttpGet("product-recommendation/{customerId}")]
        public async Task<IActionResult> RecommendProducts(string customerId)
        {
            return Ok(await _repo.RecommendProducts(customerId));
        }

        [HttpPost("import-products")]
        public async Task<IActionResult> Import(WareHouseVM vm)
        {
            return Ok(await _repo.Import(vm));
        }

        [HttpGet("get-detail-import/{productID}")]
        public async Task<IActionResult> GetDetails(string productID)
        {
            return Ok(await _repo.GetDetailImport(productID));
        }
    }
}
