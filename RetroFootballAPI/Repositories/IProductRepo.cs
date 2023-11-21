using RetroFootballAPI.Models;

namespace RetroFootballAPI.Repositories
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetByID(string id);
        Task<IEnumerable<Product>> BestSeller();
        Task<IEnumerable<Product>> TopSelling(int month, int year);
        Task<IEnumerable<Product>> NewArrivals();
        Task<Product> Add(Product product);
        Task<Product> Update(Product product);
        Task<Product> Delete(string id);
    }
}
