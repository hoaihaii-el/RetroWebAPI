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
            //detail.Product = product;

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

        public async Task<OrderDetailsGetVM> GetByOrderID(int orderID)
        {
            var orders = await _context.OrderDetails
                .Where(d => d.OrderID == orderID)
                .ToListAsync();
            OrderDetailsGetVM vm = new OrderDetailsGetVM();
            vm.Products = new List<OrderProductVM>();
            var order = await _context.Orders.FindAsync(orderID);
            if (order.VoucherID != null)
            {
                var voucher = await _context.Voucher.FindAsync(order.VoucherID);
                if (voucher != null)
                {
                    order.Voucher = voucher;
                }
            }
            
            if (order != null)
            {
                vm.Order = order;
            }
            foreach(var item in orders)
            {
                var product = await _context.Products.FindAsync(item.ProductID);
                if (product != null)
                {
                    var orderProduct = new OrderProductVM();
                    orderProduct.Product = product;
                    orderProduct.Size = item.Size;
                    orderProduct.Quantity = item.Quantity;
                    orderProduct.didFeedback = item.didFeedback;
                    orderProduct.OrderID = orderID;
                    vm.Products.Add(orderProduct);
                }
            }
            return vm;
        }
        public async Task<List<OrderProductVM>> getUnReviewedProducts(string customerID)
        {
            var orders = await _context.Orders
                        .Where(o => o.CustomerID == customerID && o.Status == "Completed")
                        .ToListAsync();
            List<OrderProductVM> products = new List<OrderProductVM>();

            foreach (var order in orders)
            {
                var item = await GetByOrderID(order.ID);
                var filteredItems = item.Products.FindAll(item => item.didFeedback == false);
                products.AddRange(filteredItems);
            }
            return products;
        }
    }
}
