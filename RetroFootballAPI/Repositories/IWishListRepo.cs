using RetroFootballAPI.ViewModels;
using RetroFootballAPI.Models;

namespace RetroFootballAPI.Repositories
{
    public interface IWishListRepo
    {
        Task<WishList> Add(WishListVM wishList);
        Task<WishList> Delete(string customerID, string productID);
        Task<IEnumerable<WishList>> GetByCustomer(string customerID);
    }
}
