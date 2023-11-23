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
        public async Task<IActionResult> GetAllMessages(int roomID)
        {
            return Ok(await _repo.GetAllMessages(roomID));
        }

        [HttpPost("add-new-room")]
        public async Task<IActionResult> AddRoom(ChatRoomVM room)
        {
            return Ok(await _repo.AddRoom(room));
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(MessageVM message, string userID)
        {
            if (!message.IsCustomerSend)
            {
                await _hub.Clients.User(userID).SendAsync("ReceiveMessage", message);
            }
            else
            {
                // get list admin ID, if admin is connected -> send message
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
