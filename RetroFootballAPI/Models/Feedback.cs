using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class Feedback
    {
        [Key, MaxLength(50), Required]
        public string? CustomerID { get; set; }
        [Key, MaxLength(50), Required]
        public string? ProductID { get; set; }
        public string? Comment { get; set; } 
        public int Point {  get; set; }
        public DateTime Date { get; set; }
        public bool IsHaveMedia { get; set; }

        // Đối tượng liên quan đến Customer
        public Customer? Customer { get; set; }

        // Đối tượng liên quan đến Product
        public Product? Product { get; set; }
    }
}
