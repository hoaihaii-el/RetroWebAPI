namespace RetroFootballAPI.ViewModels
{
    public class OrderVM
    {
        public int ID { get; set; }
        public string? CustomerID { get; set; }
        public DateTime TimeCreate { get; set; }
        public decimal Value { get; set; }
        public string? PayMethod { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string? DeliveryMethod { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Note { get; set; }
    }
}
