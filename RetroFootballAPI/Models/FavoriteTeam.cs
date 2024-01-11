using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class FavoriteTeam
    {
        [Key, MaxLength(50)]
        public string? CustomerID { get; set; }
        [Key]
        public string? TeamName { get; set; }
    }
}
