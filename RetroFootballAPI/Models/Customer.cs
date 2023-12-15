using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class Customer
    {
        [Key, MaxLength(50), Required]
        public string? ID { get; set; }

        [Required, MaxLength(50)]
        public string? Name { get; set; }

        [Required, MaxLength(20)]
        public string? Phone { get; set; }

        public bool Male { get; set; }
        public string? Address { get; set; }
        public DateTime DateBirth { get; set; }
        public string? Avatar { get; set; }
    }
}
