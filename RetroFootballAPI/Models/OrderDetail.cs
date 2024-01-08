using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class OrderDetail
    {
        [Key, Required]
        public int OrderID { get; set; }
        [Key, MaxLength(50), Required]
        public string? ProductID { get; set; }
        [Key, MaxLength(10), Required]
        public string? Size { get; set; }
        public int Quantity { get; set; }

        //optional
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
