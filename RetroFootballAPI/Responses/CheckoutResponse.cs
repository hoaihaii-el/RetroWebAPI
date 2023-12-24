using RetroFootballAPI.Models;

namespace RetroFootballAPI.Responses
{
    public class CheckoutResponse
    {
        public int Items { get; set; }
        public decimal Total { get; set; }
        public Voucher? Voucher { get; set; }
    }
}
