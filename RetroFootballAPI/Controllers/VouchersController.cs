using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;

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

        [Authorize(Roles = AppRole.Admin)]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }

        [Authorize]
        [HttpGet("filter-by")]
        public async Task<IActionResult> Filter(string param)
        {
            return Ok(await _repo.Filter(param));
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
