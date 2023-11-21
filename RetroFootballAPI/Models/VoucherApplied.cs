using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class VoucherApplied
    {
        [Key, MaxLength(50), Required]
        public string? VoucherID { get; set; }
        [Key, MaxLength(50), Required]
        public string? ProductID { get; set; }
    }
}
