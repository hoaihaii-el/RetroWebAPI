namespace RetroFootballAPI.ViewModels
{
    public class OrderVM
    {
        public string? CustomerID { get; set; }
        public decimal Value { get; set; }
        public string? PayMethod { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? Note { get; set; }
        public decimal Shipping { get; set; }
        public string? VoucherID  { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
