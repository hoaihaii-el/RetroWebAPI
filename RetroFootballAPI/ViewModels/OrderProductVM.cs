using RetroFootballAPI.Models;

namespace RetroFootballAPI.ViewModels
{
    public class OrderProductVM
    {
        public Product Product { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public bool didFeedback { get; set; }
        public int OrderID { get; set; }
    }
}
