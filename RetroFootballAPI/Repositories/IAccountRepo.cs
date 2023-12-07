using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using RetroFootballAPI.Models;
using RetroFootballAPI.Responses;
using RetroFootballAPI.ViewModels;
using System.Security.Claims;

namespace RetroFootballAPI.Repositories
{
    public interface IAccountRepo
    {
        Task<CustomerVM> Register(Register user);
        Task NewAdminAccount(Register admin);
        Task<LoginResponse> Login(Login user);
        Task<string> LoginByGoogle(string email);
        Task<Customer> ReadMe(ClaimsPrincipal claim);
        Task Logout();
    }
}
