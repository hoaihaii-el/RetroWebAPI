using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Repositories;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatRepo _repo;

        public StatsController(IStatRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("get-revenue-this/{time}")]
        public async Task<IActionResult> GetRevenue(int time)
        {
            return Ok(await _repo.GetRevenue(time));
        }

        [HttpGet("revenue-by-month")]
        public async Task<IActionResult> RevenueByMonth()
        {
            return Ok(await _repo.RevenueByMonths());
        }
    }
}
