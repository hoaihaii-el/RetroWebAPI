﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using RetroFootballAPI.Models;
using RetroFootballAPI.ViewModels;
using System.Security.Claims;

namespace RetroFootballAPI.Hubs
{
    public class ChatHub : Hub
    {
        public static Dictionary<string, string> userConnections = new Dictionary<string, string>();
        private readonly UserManager<AppUser> _userManager;

        public ChatHub(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} has joined to ChatHub");

            return base.OnConnectedAsync();
        }

        public async Task Connect(string customerID)
        {
            try
            {
                userConnections.Add(customerID, Context.ConnectionId);
            }
            catch
            {
                userConnections[customerID] = Context.ConnectionId;
            }
            await Clients.Caller.SendAsync("ReceiveMessage", "Admin", "Welcome to the chat!");
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"{Context.ConnectionId} has left the ChatHub");

            var userID = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            userConnections.Remove(userID ?? "");

            return base.OnDisconnectedAsync(exception);
        }
    }
}
