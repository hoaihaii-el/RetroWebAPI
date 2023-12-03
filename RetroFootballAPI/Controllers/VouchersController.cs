using RetroFootballAPI.Models;
using Microsoft.AspNetCore.Mvc;
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


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }


        [HttpGet("get-available")]
        public async Task<IActionResult> GetAvailable()
        {
            return Ok(await _repo.GetAvailable());
        }


        [HttpGet("get-by-id/{voucherID}")]
        public async Task<IActionResult> GetVoucher(string voucherID)
        {
            return Ok(await _repo.GetById(voucherID));
        }

        [HttpGet("get-voucher-applied/{productID}")]
        public async Task<IActionResult> GetVoucherApplied(string productID)
        {
            return Ok(await _repo.GetVoucherApplied(productID));
        }


        [HttpPut("update/{voucher}")]
        public async Task<IActionResult> Update(Voucher voucher)
        {
            return Ok(await _repo.Update(voucher));
        }


        [HttpPost("new-voucher")]
        public async Task<IActionResult> Add(VoucherVM voucher, [FromQuery] List<string> productsApplied)
        {
            return Ok(await _repo.Add(voucher, productsApplied));
        }


        [HttpDelete("delete/{voucherID}")]
        public async Task<IActionResult> DeleteVoucher(string voucherID)
        {
            return Ok(await _repo.Delete(voucherID));
        }
    }
}
