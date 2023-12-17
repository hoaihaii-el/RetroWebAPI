namespace RetroFootballAPI.ViewModels
{
    public class OrderVM
    {
        public string? CustomerID { get; set; }
        public decimal Value { get; set; }
        public string? PayMethod { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? Note { get; set; }
    }
}
