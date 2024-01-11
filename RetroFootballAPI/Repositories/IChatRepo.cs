using RetroFootballAPI.Models;
using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Repositories
{
    public interface IChatRepo
    {
        Task<ChatRoom> AddRoom(ChatRoomVM room);
        Task<IEnumerable<Message>> GetAllMessages(string customerID);
        Task<Message> AddMessage(MessageVM message);
        Task<Message> ReadMessage(int messageID);
        Task<IEnumerable<string>> GetAdminsId();
        Task<IEnumerable<ChatRoom>> GetRooms();
    }
}
