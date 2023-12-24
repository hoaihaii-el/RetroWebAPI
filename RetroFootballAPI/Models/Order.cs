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
        [MaxLength(500)]
        public string? Note { get; set; }
        public decimal? Shipping { get; set; }
        [MaxLength(50)]
        public string? VoucherID { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsPaid { get; set; }

        //optional
        public Customer? Customer { get; set; }
        public Voucher? Voucher { get; set; }
    }
}
