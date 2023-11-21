using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackRepo _repo;

        public FeedbacksController(IFeedbackRepo repo)
        {
            _repo = repo;
        }


        [HttpGet("{productID}")]
        public async Task<ActionResult> GetAll(string productID)
        {
            return Ok(await _repo.GetAll(productID));
        }


        [HttpGet("point/{productID}")]
        public async Task<IActionResult> GetAvgPoint(string productID)
        {
            return Ok(await _repo.GetAvgPoint(productID));
        }


        [HttpPost]
        public async Task<ActionResult> Add(FeedbackVM feedback)
        {
            return Ok(await _repo.Add(feedback));
        }


        [HttpPut("{feedback}")]
        public async Task<IActionResult> Update(FeedbackVM feedback)
        {
            return Ok(await _repo.Update(feedback));
        }
    }
}
