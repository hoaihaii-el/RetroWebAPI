namespace RetroFootballAPI.ViewModels
{
    public class UpdateAvatarVM
    {
        public string? Email { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
