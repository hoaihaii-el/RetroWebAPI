using Microsoft.AspNetCore.Identity;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;
using RetroFootballWeb.Repository;

namespace RetroFootballAPI.Services
{
    public class AccountRepo : IAccountRepo
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTManager _JWTManager;
        private readonly DataContext _context;

        public AccountRepo(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            JWTManager JWTManager,
            DataContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _JWTManager = JWTManager;
            _context = context;
            _roleManager = roleManager;
        }


        public async Task<string> Login(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                throw new ArgumentNullException("User not found");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, login.Password);

            if (!passwordValid)
            {
                return string.Empty;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return _JWTManager.GenerateToken(user.Email, userRoles.FirstOrDefault() ?? "");
        }

        public async Task<CustomerVM> Register(Register register)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = register.UserName,
                Email = register.Email,
                PhoneNumber = register.Phone
            };

            var customer = new Customer
            {
                ID = user.Id,
                Name = user.UserName,
                Phone = user.PhoneNumber
            };

            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            await _userManager.CreateAsync(user, register.Password);

            if (!await _roleManager.RoleExistsAsync(AppRole.Customer))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(AppRole.Customer)
                );
            }

            await _userManager.AddToRoleAsync(user, AppRole.Customer);

            return new CustomerVM
            {
                ID = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber
            };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> LoginByGoogle(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return _JWTManager.GenerateToken(email, AppRole.Customer);
            }

            var newUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email
            };

            await _userManager.CreateAsync(newUser);

            await _userManager.AddToRoleAsync(newUser, AppRole.Customer);

            return _JWTManager.GenerateToken(email, AppRole.Customer);
        }

        public async Task NewAdminAccount(Register register)
        {
            var admin = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = register.UserName,
                Email = register.Email
            };

            await _userManager.CreateAsync(admin, register.Password);

            if (!await _roleManager.RoleExistsAsync(AppRole.Admin))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(AppRole.Admin)
                );
            }

            await _userManager.AddToRoleAsync(admin, AppRole.Admin);
        }
    }
}
