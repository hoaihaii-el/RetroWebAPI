using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Enter your Username!")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Enter your Email!")]
        [EmailAddress(ErrorMessage = "Invalid Email!")]
        public string? Email { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Enter your Password!")]
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public List<string>? Favorites { get; set; }
    }
}
