using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetroFootballAPI.Models
{
    public class Product
    {
        [Key, MaxLength(50), Required]
        public string? ID { get; set; }
        [Required, MaxLength(100)]
        public string? Name { get; set; }
        [Required, MaxLength(100)]
        public string? Club { get; set; }
        [Required, MaxLength(100)]
        public string? Nation { get; set; }
        [Required, MaxLength(100)]
        public string? Season { get; set; }
        public decimal Price { get; set; }
        public int SizeS { get; set; }
        public int SizeM { get; set; }
        public int SizeL { get; set; }
        public int SizeXL { get; set; }
        [MaxLength(10)]
        public string? Status { get; set; }
        public DateTime TimeAdded { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public double Point { get; set; }
        [NotMapped]
        public int Sold { get; set; }
        [NotMapped]
        public string? GroupName { get; set; } 
        public string? UrlMain { get; set; }
        public string? UrlSub1 { get; set; }
        public string? UrlSub2 { get; set; }
        public string? UrlThumb { get; set; }
    }
}
