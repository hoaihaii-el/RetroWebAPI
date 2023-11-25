﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepo _repo;

        public AccountsController(IAccountRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register register)
        {
            var result = await _repo.Register(register);

            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return Unauthorized();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login user)
        {
            var result = await _repo.Login(user);

            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var redirectUri = "/api/Account/ExternalLoginResponse";

            var properties = _repo.GoogleLogin(redirectUri);

            return new ChallengeResult("Google", properties);
        }

        [HttpGet("ExternalLoginResponse")]
        public async Task<IActionResult> ExternalLoginResponse()
        {
            var result = await _repo.ExternalLoginResponse();

            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
