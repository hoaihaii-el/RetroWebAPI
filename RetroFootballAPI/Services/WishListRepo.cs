using RetroFootballAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballWeb.Repository;

namespace RetroFootballAPI.Services
{
    public class WishListRepo : IWishListRepo
    {
        private readonly DataContext _context;

        public WishListRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<WishList> Add(WishListVM wishListVM)
        {
            var wishList = new WishList
            {
                ProductID = wishListVM.ProductID,
                CustomerID = wishListVM.CustomerID,
            };

            var customer = await _context.Customers.FindAsync(wishListVM.CustomerID);
            var product = await _context.Products.FindAsync(wishListVM.ProductID);

            if (customer == null || product == null)
            {
                throw new KeyNotFoundException();
            }

            wishList.Customer = customer;
            wishList.Product = product;

            _context.WishLists.Add(wishList);
            await _context.SaveChangesAsync();

            return wishList;
        }

        public async Task<WishList> Delete(string customerID, string productID)
        {
            var wishList = await _context.WishLists.FindAsync(customerID, productID);

            if (wishList == null)
            {
                throw new KeyNotFoundException();
            }

            _context.WishLists.Remove(wishList);
            await _context.SaveChangesAsync();

            return wishList;
        }

        public async Task<IEnumerable<WishList>> GetByCustomer(string customerID)
        {
            return await _context.WishLists
                .Where(w => w.CustomerID == customerID)
                .ToListAsync();
        }
    }
}
