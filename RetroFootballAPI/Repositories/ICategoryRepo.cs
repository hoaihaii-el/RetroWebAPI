using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Repositories
{
    public interface ICategoryRepo
    {
        Task<IEnumerable<CategoryVM>> GetClubs();
        Task<IEnumerable<CategoryVM>> GetNations();
        Task<IEnumerable<CategoryVM>> GetSeasons();
    }
}
