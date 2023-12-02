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
            return Ok(await _repo.Register(register));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login user)
        {
            try
            {
                var result = await _repo.Login(user);

                if (string.IsNullOrEmpty(result))
                {
                    return Unauthorized();
                }

                return Ok(result);
            }
            catch (ArgumentNullException)
            {
                return Ok(
                    new { message = "User not found" }
                );
            }
        }

        [HttpPost("new-admin")]
        public async Task<IActionResult> NewAdminAccount(Register admin)
        {
            await _repo.NewAdminAccount(admin);
            return Ok(
                new { success = true }
            );
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogOut()
        {
            await _repo.Logout();
            return Ok(
                new { success = true }
            );
        }

        [HttpPost("login-by-google")]
        public async Task<IActionResult> LoginByGoogle(string email)
        {
            var result = await _repo.LoginByGoogle(email);

            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
