using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class DeliveryInfo
    {
        [Key, MaxLength(50), Required]
        public string? CustomerID { get; set; }
        [Key, Required]
        public int Priority { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
        [MaxLength(20)]
        public string? Phone { get; set; }

        //optional
        public Customer? Customer { get; set; }
    }
}
