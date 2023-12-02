﻿using RetroFootballAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballWeb.Repository;

namespace RetroFootballAPI.Services
{
    public class CartRepo : ICartRepo
    {
        private readonly DataContext _context;

        public CartRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<Cart> AddToCart(CartVM cart)
        {
            var model = await _context.Carts.FindAsync(cart.CustomerID, cart.ProductID, cart.Size);

            if (model != null)
            {
                model.Quantity += cart.Quantity;
                _context.Carts.Update(model);
                await _context.SaveChangesAsync();
                return model;
            }

            var item = new Cart
            {
                CustomerID = cart.CustomerID,
                ProductID = cart.ProductID,
                Size = cart.Size,
                Quantity = cart.Quantity
            };

            var customer = await _context.Customers.FindAsync(cart.CustomerID);
            var product = await _context.Products.FindAsync(cart.ProductID);

            if (customer == null || product == null)
            {
                throw new KeyNotFoundException();
            }

            item.Customer = customer;
            item.Product = product;

            _context.Carts.Add(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task ClearCart(string customerID)
        {
            var carts = await _context.Carts
                .Where(c => c.CustomerID == customerID)
                .ToListAsync();

            _context.RemoveRange(carts);
            await _context.SaveChangesAsync();
        }


        public async Task<Cart> DecreaseQuantity(string customerID, string productID, string size)
        {
            var cart = await _context.Carts.FindAsync(customerID, productID, size);

            if (cart == null)
            {
                throw new KeyNotFoundException();
            }

            if (cart.Quantity > 0)
            {
                cart.Quantity--;
            }

            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<IEnumerable<Cart>> GetCarts(string customerID)
        {
            return await _context.Carts
                .Where(c => c.CustomerID == customerID)
                .ToListAsync();
        }

        public async Task<decimal> GetCartTotal(string customerID)
        {
            return await _context.Carts
                .Where(c => c.CustomerID == customerID)
                .Select(c => c.Product.Price * c.Quantity)
                .SumAsync();
        }

        public async Task<int> GetTotalItems(string customerID)
        {
            return await _context.Carts
                .Where(c => c.CustomerID == customerID)
                .Select(c => c.Quantity)
                .SumAsync();
        }


        public async Task<Cart> IncreaseQuantity(string customerID, string productID, string size)
        {
            var cart = await _context.Carts.FindAsync(customerID, productID, size);

            if (cart == null)
            {
                throw new KeyNotFoundException();
            }

            cart.Quantity++;

            _context.Carts.Update(cart);

            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> RemoveFromCart(string customerID, string productID, string size)
        {
            var cart = await _context.Carts.FindAsync(customerID, productID, size);

            if (cart == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Carts.Remove(cart);

            _context.SaveChanges();

            return cart;
        }
    }
}
