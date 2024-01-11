using Microsoft.AspNetCore.Authorization;
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


        [HttpGet("get-all/{productID}")]
        public async Task<ActionResult> GetAll(string productID)
        {
            return Ok(await _repo.GetAll(productID));
        }

        [HttpGet("get-by-customer/{customerID}")]
        public async Task<ActionResult> GetAllByCustomerID(string customerID)
        {
            return Ok(await _repo.GetAllByCustomerID(customerID));
        }


        [HttpGet("point/{productID}")]
        public async Task<IActionResult> GetAvgPoint(string productID)
        {
            return Ok(await _repo.GetAvgPoint(productID));
        }

        [Authorize]
        [HttpPost("new-feedback")]
        public async Task<ActionResult> Add([FromBody]FeedbackVM feedback)
        {
            return Ok(await _repo.Add(feedback));
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]FeedbackVM feedback)
        {
            return Ok(await _repo.Update(feedback));
        }
    }
}
