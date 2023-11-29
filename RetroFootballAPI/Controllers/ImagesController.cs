using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Repositories;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepo _repo;

        public ImagesController(IImageRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("AddImage")]
        public async Task<IActionResult> AddImage(IFormFile file)
        {
            return Ok(await _repo.AddImage(file));
        }
    }
}
