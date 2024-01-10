
using RetroFootballAPI.Models;

namespace RetroFootballAPI.ViewModels
{
    public class OrderDetailsGetVM
    {
        public Order Order { get; set; }
        public List<Product> Products { get; set; }
    }
}
