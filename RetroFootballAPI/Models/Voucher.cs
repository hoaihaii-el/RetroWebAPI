using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class Voucher
    {
        [Key, MaxLength(50), Required]
        public string? ID { get; set; }
        [Required, MaxLength(100)]
        public string? Name { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public decimal Value { get; set; }
    }
}
