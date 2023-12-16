using RetroFootballAPI.Models;

namespace RetroFootballAPI.Responses
{
    public class LoginResponse
    {
        public string? access_token { get; set; }
        public string? id { get; set; }
        public string? email { get; set; }
        public string? name { get; set; }
        public string? role { get; set; }
        public Customer? Customer { get; set; }
    }
}
