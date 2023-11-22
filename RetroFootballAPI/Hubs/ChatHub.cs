using Microsoft.AspNetCore.SignalR;

namespace RetroFootballAPI.Hubs
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} has joined to ChatHub");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"{Context.ConnectionId} has left the ChatHub");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
