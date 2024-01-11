using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.StaticService;
using RetroFootballAPI.ViewModels;
using RetroFootballWeb.Repository;

namespace RetroFootballAPI.Services
{
    public class ChatRepo : IChatRepo
    {
        private readonly DataContext _context;
        public ChatRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<ChatRoom> AddRoom(ChatRoomVM room)
        {
            var newRoom = new ChatRoom
            {
                CustomerID = room.CustomerID
            };

            var customer = await _context.Customers.FindAsync(room.CustomerID);

            if (customer == null)
            {
                throw new KeyNotFoundException();
            }

            newRoom.Customer = customer;

            _context.ChatRooms.Add(newRoom);
            await _context.SaveChangesAsync();

            return newRoom;
        }

        public async Task<IEnumerable<ChatRoom>> GetRooms()
        {
            var rooms = await _context.Messages.OrderByDescending(r => r.SendTime).Select(r => r.RoomID).Distinct().ToListAsync();

            var result = new List<ChatRoom>();
            foreach (var room in rooms)
            {
                var roomInfo = await _context.ChatRooms.Where(r => r.RoomID == room).FirstOrDefaultAsync();
                result.Add(new ChatRoom
                {
                    RoomID = room ?? 0,
                    CustomerID = roomInfo.CustomerID,
                    Customer = await _context.Customers.FindAsync(roomInfo.CustomerID)
                });
            }
            return result;
        }

        public async Task<Message> AddMessage(MessageVM message)
        {
            var newMsg = new Message
            {
                Content = message.Content,
                SendTime = DateTime.Now,
                IsReaded = false,
                IsCustomerSend = message.IsCustomerSend,
            };

            var room = await _context.ChatRooms
                .Where(m => m.CustomerID == message.CustomerID)
                .FirstOrDefaultAsync();

            if (room == null)
            {
                throw new KeyNotFoundException();
            }

            newMsg.Room = room;
            newMsg.Media = await UploadImage.Instance.UploadAsync(message.CustomerID + Guid.NewGuid().ToString(), message.Media);

            _context.Messages.Add(newMsg);
            await _context.SaveChangesAsync();

            return newMsg;
        }

        public async Task<Message> ReadMessage(int messageID)
        {
            var message = await _context.Messages.FindAsync(messageID);

            if (message == null)
            {
                throw new KeyNotFoundException();
            }

            message.ReadTime = DateTime.Now;
            message.IsReaded = true;

            _context.Messages.Update(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<IEnumerable<Message>> GetAllMessages(string customerID)
        {
            var room = await _context.ChatRooms.Where(r => r.CustomerID == customerID).FirstOrDefaultAsync();

            return await _context.Messages
                .Where(m => m.RoomID == room.RoomID)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAdminsId()
        {
            var roleAdminID = await _context.Roles
                .Where(r => r.Name.Contains("Admin"))
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            return await _context.UserRoles
                .Where(u => u.RoleId == roleAdminID)
                .Select(u => u.UserId)
                .ToListAsync();
        }
    }
}
