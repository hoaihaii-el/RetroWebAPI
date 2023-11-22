using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Hubs;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;
using RetroFootballWeb.Repository;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace RetroFootballAPI.Services
{
    public class ChatRepo : IChatRepo
    {
        private readonly DataContext _context;
        private readonly SqlTableDependency<Message> _messageTableListener;
        private readonly SqlTableDependency<ChatRoom> _chatRoomTableListener;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IConfiguration _config;

        public ChatRepo(DataContext context, IHubContext<ChatHub> hubContext, IConfiguration config)
        {
            _context = context;
            _hubContext = hubContext;
            _config = config;

            _messageTableListener = new SqlTableDependency<Message>(_config["ConnectionStrings:ConnectedDB"], "Messages");
            _messageTableListener.OnChanged += MessagesChanged;
            _messageTableListener.Start();

            _chatRoomTableListener = new SqlTableDependency<ChatRoom>(_config["ConnectionStrings:ConnectedDB"], "ChatRooms");
            _chatRoomTableListener.OnChanged += RoomsChanged;
            _chatRoomTableListener.Start();
        }

        private async void MessagesChanged(object sender, RecordChangedEventArgs<Message> e)
        {
            var newMessage = e.Entity;

            if (newMessage == null)
            {
                return;
            }

            var messages = GetAllMessages(newMessage.RoomID ?? 1);
            var room = await _context.ChatRooms.FindAsync(newMessage.RoomID);

            if (!newMessage.IsCustomerSend)
            {
                await _hubContext.Clients.All.SendAsync("NewMessage", room?.CustomerID, messages);
            }
        }

        private void RoomsChanged(object sender, RecordChangedEventArgs<ChatRoom> e)
        {
            throw new NotImplementedException();
        }


        public async Task<ChatRoom> AddRoom(ChatRoomVM room)
        {
            var newRoom = new ChatRoom
            {
                CustomerID = room.CustomerID,
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

        public async Task<Message> AddMessage(MessageVM message)
        {
            var newMsg = new Message
            {
                RoomID = message.RoomID,
                Content = message.Content,
                ContentType = message.ContentType,
                SendTime = message.SendTime,
                ReadTime = message.ReadTime,
                IsReaded = message.IsReaded,
                IsCustomerSend = message.IsCustomerSend,
            };

            var room = await _context.ChatRooms.FindAsync(newMsg.RoomID);

            if (room == null)
            {
                throw new KeyNotFoundException();
            }

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

        public async Task<IEnumerable<Message>> GetAllMessages(int roomID)
        {
            return await _context.Messages
                .Where(m => m.RoomID == roomID)
                .ToListAsync();
        }
    }
}
