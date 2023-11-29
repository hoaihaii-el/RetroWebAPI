using CloudinaryDotNet.Actions;

namespace RetroFootballAPI.Repositories
{
    public interface IImageRepo
    {
        Task<ImageUploadResult> AddImage(IFormFile file);
    }
}
