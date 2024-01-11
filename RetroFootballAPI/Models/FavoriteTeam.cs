using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class FavoriteTeam
    {
        [Required, MaxLength(50)]
        public string? CustomerID { get; set; }
        public string? TeamName { get; set; }
    }
}
