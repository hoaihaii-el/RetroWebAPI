﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using RetroFootballAPI.Models;

namespace RetroFootballAPI.Repositories
{
    public interface IAccountRepo
    {
        Task<IdentityResult> Register(Register user);
        Task<string> Login(Login user);
        Task<string> LoginByGoogle(string email);
        Task Logout();
    }
}
