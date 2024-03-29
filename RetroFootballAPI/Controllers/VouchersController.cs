﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly IAccountRepo _accRepo;

        public VouchersController(IVoucherRepo repo, IAccountRepo accRepo)
        {
            _repo = repo;
            _accRepo = accRepo;
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
            var customer = await _accRepo.ReadMe(User);

            return Ok(await _repo.Filter(param, customer.ID ?? ""));
        }

        [Authorize]
        [HttpGet("get-by-id/{voucherID}")]
        public async Task<IActionResult> GetVoucher(string voucherID)
        {
            return Ok(await _repo.GetById(voucherID));
        }

        [Authorize]
        [HttpPut("update/{ID}")]
        public async Task<IActionResult> Update(string ID, VoucherVM voucher)
        {
            return Ok(await _repo.Update(ID, voucher));
        }

        [Authorize(Roles = AppRole.Admin)]
        [HttpPost("new-voucher")]
        public async Task<IActionResult> Add(VoucherVM voucher)
        {
            return Ok(await _repo.Add(voucher));
        }

        [Authorize(Roles = AppRole.Admin)]
        [HttpGet("search-by-name")]
        public async Task<IActionResult> Search(string name)
        {
            return Ok(await _repo.SearchByName(name));
        }

        [Authorize(Roles = AppRole.Admin)]
        [HttpDelete("delete/{voucherID}")]
        public async Task<IActionResult> DeleteVoucher(string voucherID)
        {
            return Ok(await _repo.Delete(voucherID));
        }
    }
}
