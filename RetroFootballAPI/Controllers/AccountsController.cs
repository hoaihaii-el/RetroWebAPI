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
            return Ok(new
            {
                message = "Success",
                data = await _repo.Register(register)
            });
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
                return StatusCode(StatusCodes.Status422UnprocessableEntity);
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
                data = await _repo.ReadMe(HttpContext.User)
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
    }
}
