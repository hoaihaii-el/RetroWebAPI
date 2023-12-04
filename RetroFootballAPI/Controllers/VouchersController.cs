using RetroFootballAPI.Models;
using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }

        [Authorize]
        [HttpGet("get-available")]
        public async Task<IActionResult> GetAvailable()
        {
            return Ok(await _repo.GetAvailable());
        }

        [Authorize]
        [HttpGet("get-by-id/{voucherID}")]
        public async Task<IActionResult> GetVoucher(string voucherID)
        {
            return Ok(await _repo.GetById(voucherID));
        }

        [Authorize]
        [HttpGet("get-voucher-applied/{productID}")]
        public async Task<IActionResult> GetVoucherApplied(string productID)
        {
            return Ok(await _repo.GetVoucherApplied(productID));
        }

        [Authorize]
        [HttpPut("update/{voucher}")]
        public async Task<IActionResult> Update([FromForm] Voucher voucher)
        {
            return Ok(await _repo.Update(voucher));
        }

        [Authorize(Roles = AppRole.Admin)]
        [HttpPost("new-voucher")]
        public async Task<IActionResult> Add([FromForm] VoucherVM voucher, [FromQuery] List<string> productsApplied)
        {
            return Ok(await _repo.Add(voucher, productsApplied));
        }

        [Authorize(Roles = AppRole.Admin)]
        [HttpDelete("delete/{voucherID}")]
        public async Task<IActionResult> DeleteVoucher(string voucherID)
        {
            return Ok(await _repo.Delete(voucherID));
        }
    }
}
