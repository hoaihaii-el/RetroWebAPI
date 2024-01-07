using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RetroFootballAPI.Hubs;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hub;
        private readonly IChatRepo _repo;

        public ChatController(IChatRepo repo, IHubContext<ChatHub> hub) 
        {
            _repo = repo;
            _hub = hub;
        }

        [HttpGet("get-messages/{roomID}")]
        [Authorize]
        public async Task<IActionResult> GetAllMessages(int roomID)
        {
            return Ok(await _repo.GetAllMessages(roomID));
        }

        [HttpPost("add-new-room")]
        [Authorize]
        public async Task<IActionResult> AddRoom([FromBody] ChatRoomVM room)
        {
            return Ok(await _repo.AddRoom(room));
        }

        [HttpPost("send-message")]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] MessageVM message)
        {
            if (!message.IsCustomerSend)
            {
                if (ChatHub.userConnections.ContainsKey(message.CustomerID))
                {
                    await _hub.Clients.Client(ChatHub.userConnections[message.CustomerID])
                        .SendAsync("ReceiveMessage", message);
                }
            }
            else
            {
                var adminIDs = await _repo.GetAdminsId();

                foreach (var adminID in adminIDs)
                {
                    if (ChatHub.userConnections.ContainsKey(adminID))
                    {
                        await _hub.Clients.Client(ChatHub.userConnections[adminID])
                        .SendAsync("ReceiveMessage", message);
                    }
                }
            }

            return Ok(await _repo.AddMessage(message));
        }

        [HttpPut("read-message/{messageID}")]
        public async Task<IActionResult> ReadMessage(int messageID)
        {
            return Ok(await _repo.ReadMessage(messageID));
        }
    }
}
