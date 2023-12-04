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
        Task<IEnumerable<Product>> GetByPrice(decimal min, decimal max, int page, int productPerPage);
        Task<IEnumerable<Product>> GetProductByPage(int page, int productPerPage);
        Task<IEnumerable<Product>> GetByCheckBox(List<string> value, int page, int productPerPage);
        Task<IEnumerable<Product>> GetBySearch(string value, int page, int productPerPage);
        Task<Product> Add(ProductVM product);
        Task<Product> Update(ProductVM product);
        Task<Product> Delete(string id);
    }
}
