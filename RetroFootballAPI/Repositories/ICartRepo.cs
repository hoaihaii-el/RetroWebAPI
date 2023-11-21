using RetroFootballAPI.Models;
using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Repositories
{
    public interface ICartRepo
    {
        Task<IEnumerable<Cart>> GetCarts(string customerID);
        Task<decimal> GetCartTotal(string customerID);
        Task<int> GetTotalItems(string customerID);
        Task<Cart> AddToCart(CartVM cart);
        Task<Cart> RemoveFromCart(string customerID, string productID, string size);
        Task ClearCart(string customerID);
        Task<Cart> IncreaseQuantity(string customerID, string productID, string size);
        Task<Cart> DecreaseQuantity(string customerID, string productID, string size);
    }   
}
