using Microsoft.AspNetCore.Authorization;
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
            try
            {
                return Ok(new
                {
                    message = "Success",
                    data = await _repo.Register(register)
                });
            }
            catch(Exception ex) 
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login user)
        {
            try
            {
                var result = await _repo.Login(user);

                return Ok(new
                {
                    message = "Success",
                    data = result
                });
            }
            catch (ArgumentNullException)
            {
                return NotFound("User not found!");
            }
            catch (KeyNotFoundException)
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpGet("read-me")]
        public async Task<IActionResult> GetInfo()
        {
            return Ok(new
            {
                message = "Success",
                data = await _repo.ReadMe(User)
            });
        }

        [HttpPost("new-admin")]
        public async Task<IActionResult> NewAdminAccount(Register admin)
        {
            await _repo.NewAdminAccount(admin);
            return Ok(new 
            { 
                success = true 
            });
        }

        
        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _repo.Logout();
            return Ok(new 
            { 
                message = "Success" 
            });
        }

        [HttpPost("login-by-google")]
        public async Task<IActionResult> LoginByGoogle(string email)
        {
            var result = await _repo.LoginByGoogle(email);

            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(new
            {
                message = "Success",
                access_token = result
            });
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            try
            {
                await _repo.ChangePassword(changePassword.Email, changePassword.OldPassword, changePassword.NewPassword);
                return Ok(new
                {
                    message = "Success"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

    }
}
