using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class WareHouse
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(50), Required]
        public string? ProductID { get; set; }
        public int SizeS { get; set; }
        public int SizeM { get; set; }
        public int SizeL { get; set; }
        public int SizeXL { get; set; }
        public DateTime DateIn { get; set; }
        public string? Supplier { get; set; }
        public string? Contact { get; set; }
        public decimal Price { get; set; }
    }
}
