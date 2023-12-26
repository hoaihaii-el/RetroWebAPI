using Humanizer.Localisation.TimeToClockNotation;
using RetroFootballAPI.Models;
using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Repositories
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetByID(string id);
        Task<IEnumerable<Product>> BestSeller();
        Task<IEnumerable<Product>> TopSelling(int month, int year);
        Task<IEnumerable<Product>> NewArrivals();
        Task<IEnumerable<Product>> GetByCategory(string cate, string value, int page, int productPerPage);
        Task<IEnumerable<Product>> FilterBy(
            List<string> names,
            List<string> seasons,
            List<string> groups,
            bool club = true,
            bool nation = true,
            decimal minPrice = 0,
            decimal maxPrice = 10000000,
            string sortBy = "Name",
            bool descending = false,
            bool sizeS = false,
            bool sizeM = false,
            bool sizeL = false,
            bool sizeXL = false,
            int page = 1,
            int productPerPage = 8);
        Task<IEnumerable<Product>> GetBySearch(string value, int page, int productPerPage);
        Task<IEnumerable<string>> GetByGroup(List<string> groups);
        Task<Product> Add(ProductVM product);
        Task<Product> Update(string productID, ProductVM product);
        Task<Product> Delete(string id);
    }
}
