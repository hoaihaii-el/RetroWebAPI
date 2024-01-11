using RetroFootballAPI.Models;
using RetroFootballAPI.Responses;
using System.Security.Claims;

namespace RetroFootballAPI.Repositories
{
    public interface IAccountRepo
    {
        Task<Customer> Register(Register user);
        Task NewAdminAccount(Register admin);
        Task<LoginResponse> Login(Login user);
        Task<string> LoginByGoogle(string email);
        Task<Customer> ReadMe(ClaimsPrincipal claim);
        Task ChangePassword(string email, string oldPw, string newPw);
        Task Logout();
        Task AddFavoriteTeams(string customerID, List<string> teams);
    }
}
