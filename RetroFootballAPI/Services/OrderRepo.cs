using RetroFootballAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballWeb.Repository;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RetroFootballAPI.Services
{
    public class OrderRepo : IOrderRepo
    {
        private readonly DataContext _context;

        public OrderRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<Order> Add(OrderVM orderVM)
        {
            var order = new Order
            {
                CustomerID = orderVM.CustomerID,
                TimeCreate = DateTime.Now,
                Value = orderVM.Value,
                PayMethod = orderVM.PayMethod,
                DeliveryMethod = orderVM.DeliveryMethod,
                Note = orderVM.Note,
                Status = "Payment"
            };

            switch (orderVM.DeliveryMethod?.ToUpper())
            {
                case "NORMAL":
                    order.DeliveryDate = DateTime.Now.AddDays(4);
                    break;
                case "EXPRESS":
                    order.DeliveryDate = DateTime.Now.AddDays(2);
                    break;
                case "SAMEDAY":
                    order.DeliveryDate = DateTime.Now.AddDays(1);
                    break;
            }

            var customer = await _context.Customers.FindAsync(orderVM.CustomerID);

            if (customer == null)
            {
                throw new KeyNotFoundException();
            }

            order.Customer = customer;

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order> Delete(int orderID)
        {
            var order = await _context.Orders.FindAsync(orderID);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Remove(order);

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<IEnumerable<Order>> GetByCustomer(string customerID, int orderType)
        {
            var status = orderType == 0 ? "All" :
                         orderType == 1 ? "Pending" :
                         orderType == 2 ? "Packaging" :
                         orderType == 3 ? "Delivering" :
                         "Completed";

            if (status == "All")
            {
                return await _context.Orders
                    .Where(o => o.CustomerID == customerID)
                    .ToListAsync();
            }
            else
            {
                return await _context.Orders
                    .Where(o => o.CustomerID == customerID &&
                                o.Status == status)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetOrders(
            int orderType = 0,
            int month = 0,
            string customerID = "",
            bool today = false)
        {
            var status = orderType == 0 ? "All" :
                         orderType == 1 ? "Pending" :
                         orderType == 2 ? "Packaging" :
                         orderType == 3 ? "Delivering" :
                         "Completed";

            IEnumerable<Order> orders;

            orders = status == "All"
                ? await _context.Orders.ToListAsync()
                : await _context.Orders
                            .Where(o => o.Status == status)
                            .ToListAsync();

            if (month > 0 && month <= 12)
            {
                orders = orders.Where(o => o.TimeCreate.Month == month);
            }

            if (!string.IsNullOrEmpty(customerID))
            {
                orders = orders.Where(o => o.CustomerID == customerID);
            }

            if (today)
            {
                orders = orders.Where(o => o.TimeCreate.Date == DateTime.Now.Date);
            }

            return orders;
        }

        public async Task<Order> UpdateStatus(int orderID)
        {
            var order = await _context.Orders.FindAsync(orderID);

            if (order == null)
            {
                throw new KeyNotFoundException();
            }

            if (order.Status == "Completed")
            {
                return order;
            }

            var status = new List<string>
            {
                "Payment",
                "Pending",
                "Packaging",
                "Delivering",
                "Completed"
            };

            int index = status.IndexOf(order.Status);

            order.Status = status[index + 1];

            await _context.SaveChangesAsync();

            return order;
        }
    }
}
