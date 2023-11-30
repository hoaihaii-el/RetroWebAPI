namespace RetroFootballAPI.ViewModels
{
    public class MessageVM
    {
        public int MessageID { get; set; }
        public int? RoomID { get; set; }
        public string? Content { get; set; }
        public IFormFile? Media { get; set; }
        public DateTime SendTime { get; set; }
        public DateTime ReadTime { get; set; }
        public bool IsReaded { get; set; }
        public bool IsCustomerSend { get; set; }
    }
}
