using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RetroFootballAPI.Services
{
    public class AccountRepo : IAccountRepo
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;

        public AccountRepo(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration config) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }


        public async Task<string> Login(Login user)
        {
            var result = await _signInManager.PasswordSignInAsync(
                user.Email,
                user.Password,
                false,
                false
                );

            if (!result.Succeeded)
            {
                return string.Empty;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authenKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:SecretKey"])
                );

            var token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],
                audience: _config["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    authenKey, 
                    SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> Register(Register register)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = register.UserName,
                Email = register.Email
            };

            return await _userManager.CreateAsync(user, register.Password);
        }
    }
}
