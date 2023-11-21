using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class WishList
    {
        [Key, Required]
        public string? CustomerID { get; set; }

        [Key, Required]
        public string? ProductID { get; set; }


        // Đối tượng liên quan đến Customer
        public Customer? Customer { get; set; }

        // Đối tượng liên quan đến Product
        public Product? Product { get; set; }
    }
}
