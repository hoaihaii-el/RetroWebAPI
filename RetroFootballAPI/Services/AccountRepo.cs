﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.Responses;
using RetroFootballAPI.StaticService;
using RetroFootballAPI.ViewModels;
using RetroFootballWeb.Repository;
using System.Security.Claims;
using System.Text.RegularExpressions;

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


        public async Task<LoginResponse> Login(Login login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                throw new ArgumentNullException("User not found");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, login.Password);

            if (!passwordValid)
            {
                throw new KeyNotFoundException();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return new LoginResponse
            {
                access_token = _JWTManager.GenerateToken(user.Email, userRoles.FirstOrDefault() ?? ""),
                id = user.Id,
                name = user.UserName, 
                email = user.Email, 
                role = userRoles.FirstOrDefault(),
                Customer = await _context.Customers.FindAsync(user.Id)
            };
        }

        public async Task<Customer> Register(Register register)
        {
            var checkMail = await _userManager.FindByEmailAsync(register.Email);

            if (checkMail != null)
            {
                throw new Exception("Email existed!");
            }

            var checkUserName = await _userManager.FindByNameAsync(register.UserName);

            if (checkUserName != null)
            {
                throw new Exception("UserName existed!");
            }

            var user = new AppUser
            {
                Id = await AutoID(),
                UserName = register.UserName,
                Email = register.Email,
                PhoneNumber = register.Phone
            };

            var customer = new Customer
            {
                ID = user.Id,
                Name = user.UserName,
                Phone = user.PhoneNumber,
                Address = "",
                DateBirth = DateTime.MinValue,
                Avatar = "https://static.vecteezy.com/system/resources/previews/009/292/244/original/default-avatar-icon-of-social-media-user-vector.jpg"
            };

            _context.Customers.Add(customer);

            await _userManager.CreateAsync(user, register.Password);

            if (!await _roleManager.RoleExistsAsync(AppRole.Customer))
            {
                await _roleManager.CreateAsync(
                    new IdentityRole(AppRole.Customer)
                );
            }

            await _userManager.AddToRoleAsync(user, AppRole.Customer);

            _context.ChatRooms.Add(new ChatRoom
            {
                CustomerID= customer.ID,
                Customer = customer
            });

            await _context.SaveChangesAsync();

            return customer;
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

        public async Task AddFavoriteTeams(string customerID, List<string> teams)
        {
            foreach (var team in teams)
            {
                _context.FavoriteTeams.Add(new FavoriteTeam
                {
                    CustomerID = customerID,
                    TeamName = team
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task NewAdminAccount(Register register)
        {
            var admin = new AppUser
            {
                Id = "admin" + Guid.NewGuid().ToString(),
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

        public async Task<Customer> ReadMe(ClaimsPrincipal claim)
        {
            var email = claim.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            var customer = await _context.Customers.FindAsync(user.Id);

            if (customer == null)
            {
                throw new KeyNotFoundException();
            }

            return customer;
        }

        private async Task<string> AutoID()
        {
            var ID = "CS0001";

            var maxID = await _context.Customers
                .OrderByDescending(v => v.ID)
                .Select(v => v.ID)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(maxID))
            {
                return ID;
            }

            var numeric = Regex.Match(maxID, @"\d+").Value;

            if (string.IsNullOrEmpty(numeric))
            {
                return ID;
            }

            ID = "CS";

            numeric = (int.Parse(numeric) + 1).ToString();

            while (ID.Length + numeric.Length < 6)
            {
                ID += '0';
            }

            return ID + numeric;
        }

        public async Task ChangePassword(string email, string oldPw, string newPw)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var pwHasher = new PasswordHasher<AppUser>();
            if (pwHasher.VerifyHashedPassword(user, user.PasswordHash, oldPw) == PasswordVerificationResult.Failed)
            {
                throw new Exception("Password verified failed");
            }

            user.PasswordHash = pwHasher.HashPassword(user, newPw);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("Changed password failed!");
            }
        }
    }
}
