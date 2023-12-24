using RetroFootballAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballWeb.Repository;

namespace RetroFootballAPI.Services
{
    public class OrderDetailRepo : IOrderDetailRepo
    {
        private readonly DataContext _context;

        public OrderDetailRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<OrderDetail> Add(OrderDetailVM detailVM)
        {
            var detail = new OrderDetail
            {
                OrderID = detailVM.OrderID,
                ProductID = detailVM.ProductID,
                Size = detailVM.Size,
                Quantity = detailVM.Quantity
            };

            var order = await _context.Orders.FindAsync(detailVM.OrderID);
            var product = await _context.Products.FindAsync(detailVM.ProductID);

            if (order == null || product == null)
            {
                throw new KeyNotFoundException();
            }

            detail.Order = order;
            detail.Product = product;

            _context.OrderDetails.Add(detail);

            await _context.SaveChangesAsync();

            return detail;
        }

        public async Task<OrderDetail> Delete(string customerID, string productID, string size)
        {
            var order = await _context.OrderDetails.FindAsync(customerID, productID, size);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Remove(order);

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<IEnumerable<OrderDetail>> GetByOrderID(int orderID)
        {
            return await _context.OrderDetails
                .Where(d => d.OrderID == orderID)
                .ToListAsync();
        }
    }
}
