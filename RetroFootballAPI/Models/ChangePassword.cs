using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Enter your Email!")]
        [EmailAddress(ErrorMessage = "Invalid Email!")]
        public string? Email { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Enter your Password!")]
        public string? OldPassword { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Enter your New Password!")]
        public string? NewPassword { get; set; }
    }
}
