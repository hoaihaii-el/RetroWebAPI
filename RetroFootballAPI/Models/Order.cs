using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace RetroFootballAPI.Models
{
    public class Order
    {
        [Key, Required]
        public int ID { get; set; }
        [Required, MaxLength(50)]
        public string? CustomerID { get; set; }
        public DateTime TimeCreate { get; set; }
        public decimal Value { get; set; }
        [MaxLength(20)]
        public string? PayMethod { get; set; }
        public DateTime DeliveryDate { get; set; }
        [MaxLength(20)]
        public string? DeliveryMethod { get; set; }
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";
        [MaxLength(50)]
        public string? Note { get; set; }

        //optional
        public Customer? Customer { get; set; }
    }
}
