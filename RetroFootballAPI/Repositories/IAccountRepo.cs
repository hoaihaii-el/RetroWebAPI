using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using RetroFootballAPI.Models;
using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Repositories
{
    public interface IAccountRepo
    {
        Task<CustomerVM> Register(Register user);
        Task NewAdminAccount(Register admin);
        Task<string> Login(Login user);
        Task<string> LoginByGoogle(string email);
        Task Logout();
    }
}
