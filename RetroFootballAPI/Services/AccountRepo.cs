using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;
using RetroFootballWeb.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RetroFootballAPI.Services
{
    public class AccountRepo : IAccountRepo
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JWTManager _JWTManager;
        private readonly DataContext _context;

        public AccountRepo(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            JWTManager JWTManager,
            DataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _JWTManager = JWTManager;
            _context = context;
        }


        public async Task<string> Login(Login user)
        {
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                user.Password,
                false,
                false
                );

            if (!result.Succeeded)
            {
                return string.Empty;
            }

            var userLogged = await _context.Users
                .Where(u => u.UserName == user.UserName)
                .FirstOrDefaultAsync();

            if (userLogged == null) 
            { 
                return string.Empty;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userLogged.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            return _JWTManager.GenerateToken(userLogged.Email);
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

        public AuthenticationProperties GoogleLogin(string redirectUri)
        {
            return _signInManager
                .ConfigureExternalAuthenticationProperties("Google", redirectUri);
        }

        public async Task<string> ExternalLoginResponse()
        {
            var info = await _signInManager
                .GetExternalLoginInfoAsync();

            if (info == null)
            {
                return string.Empty;
            }

            var result = await _signInManager
                .ExternalLoginSignInAsync(
                    info.LoginProvider, 
                    info.ProviderKey, 
                    false
                );

            var email = info.Principal.FindFirst(ClaimTypes.Email)?.Value;

            var token = _JWTManager.GenerateToken(email ?? "");

            if (result.Succeeded)
            {
                return token;
            }

            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email
            };

            var create = await _userManager.CreateAsync(user);

            if (create.Succeeded)
            {
                var login = await _userManager.AddLoginAsync(user, info);

                if (login.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return token;
                }
            }

            return string.Empty;
        }

        public Task Logout()
        {
            return _signInManager.SignOutAsync();
        }
    }
}
