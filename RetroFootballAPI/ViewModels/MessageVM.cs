namespace RetroFootballAPI.ViewModels
{
    public class MessageVM
    {
        public string CustomerID { get; set; } = "";
        public string? Content { get; set; }
        public IFormFile? Media { get; set; }
        public bool IsCustomerSend { get; set; }
    }
}
