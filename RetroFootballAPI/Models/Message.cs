using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class Message
    {
        [Key, MaxLength(20), Required]
        public int MessageID { get; set; }
        [MaxLength(20), Required]
        public int? RoomID { get; set; }
        [MaxLength(1000), Required]
        public string? Content { get; set; }
        public string? Media { get; set; }
        [Required]
        public DateTime SendTime { get; set; }
        public DateTime ReadTime { get; set; }
        [Required]
        public bool IsReaded { get; set; }
        [Required]
        public bool IsCustomerSend { get; set; }
        public ChatRoom Room { get; set; } = new ChatRoom();
    }
}
