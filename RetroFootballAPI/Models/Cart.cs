using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class Cart
    {
        [Key, MaxLength(50), Required]
        public string? CustomerID { get; set; }

        [Key, MaxLength(50), Required]
        public string? ProductID { get; set; }
        [Key, MaxLength(5), Required]
        public string? Size { get; set; }

        [Required]
        public int Quantity { get; set; }


        // Optional
        public Customer Customer { get; set; } = new Customer();

        public Product Product { get; set; } = new Product();
    }
}
