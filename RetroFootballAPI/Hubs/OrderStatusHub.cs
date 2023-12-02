using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using RetroFootballAPI.Models;
using System.Security.Claims;

namespace RetroFootballAPI.Hubs
{
    public class OrderStatusHub : Hub
    {
        public static Dictionary<string, string> userConnections = new Dictionary<string, string>();
        private readonly UserManager<AppUser> _userManager;

        public OrderStatusHub(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} has joined to ChatHub");

            var userID = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            userConnections.Add(userID ?? "", Context.ConnectionId);

            return base.OnConnectedAsync();
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
