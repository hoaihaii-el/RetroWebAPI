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
            var mess = await _repo.AddMessage(message);

            if (!message.IsCustomerSend)
            {
                await _hub.Clients.All.SendAsync("ReceiveMessage", message);
            }
            else
            {
                await _hub.Clients.All.SendAsync("ReceiveMessage", message);
            }

            return Ok(mess);
        }

        [HttpPut("read-message/{messageID}")]
        public async Task<IActionResult> ReadMessage(int messageID)
        {
            return Ok(await _repo.ReadMessage(messageID));
        }
    }
}
