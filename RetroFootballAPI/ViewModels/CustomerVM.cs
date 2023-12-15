using System.ComponentModel.DataAnnotations;

namespace RetroFootballAPI.ViewModels
{
    public class CustomerVM
    {
        public string? ID { get; set; }

        public string? Name { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime DateBirth { get; set; }
        public string? Avatar { get; set; }
    }
}
