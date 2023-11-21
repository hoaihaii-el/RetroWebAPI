using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Repositories;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepo _repo;

        public CategoriesController(ICategoryRepo repo)
        {
            _repo = repo;
        }


        [HttpGet("{type}")]
        public async Task<IActionResult> GetCategory(string type)
        {
            switch(type)
            {
                case "Club":
                    return Ok(await _repo.GetClubs());
                case "Nation":
                    return Ok(await _repo.GetNations());
                case "Season":
                    return Ok(await _repo.GetSeasons());
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
