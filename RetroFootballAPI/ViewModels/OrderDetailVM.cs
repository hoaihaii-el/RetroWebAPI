namespace RetroFootballAPI.ViewModels
{
    public class OrderDetailVM
    {
        public int OrderID { get; set; }
        public string? ProductID { get; set; }
        public string? Size { get; set; }
        public int Quantity { get; set; }
        public string? VoucherID { get; set; }
    }
}
