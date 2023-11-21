using RetroFootballAPI.Models;
using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Repositories;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherRepo _repo;

        public VouchersController(IVoucherRepo repo)
        {
            _repo = repo;
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }


        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            return Ok(await _repo.GetAvailable());
        }


        [HttpGet("{voucherID}")]
        public async Task<IActionResult> GetVoucher(string voucherID)
        {
            return Ok(await _repo.GetById(voucherID));
        }


        [HttpPut("{voucher}")]
        public async Task<IActionResult> Update(Voucher voucher)
        {
            return Ok(await _repo.Update(voucher));
        }


        [HttpPost]
        public async Task<IActionResult> Add(Voucher voucher)
        {
            return Ok(await _repo.Add(voucher));
        }


        [HttpDelete("{voucherID}")]
        public async Task<IActionResult> DeleteVoucher(string voucherID)
        {
            return Ok(await _repo.Delete(voucherID));
        }
    }
}
