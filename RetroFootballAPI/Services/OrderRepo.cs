﻿using RetroFootballAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballWeb.Repository;

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
                ID = orderVM.ID,
                CustomerID = orderVM.CustomerID,
                TimeCreate = orderVM.TimeCreate,
                Value = orderVM.Value,
                PayMethod = orderVM.PayMethod,
                DeliveryDate = orderVM.DeliveryDate,
                DeliveryMethod = orderVM.DeliveryMethod,
                Status = orderVM.Status,
            };

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

        public async Task<IEnumerable<Order>> GetByCustomer(string customerID, int type)
        {
            var status = type == 0 ? "All" :
                         type == 1 ? "Pending" :
                         type == 2 ? "Packaging" :
                         type == 3 ? "Delivering" :
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

        public async Task<IEnumerable<Order>> GetByMonth(int month, int type)
        {
            var status = type == 0 ? "All" :
                         type == 1 ? "Pending" :
                         type == 2 ? "Packaging" :
                         type == 3 ? "Delivering" :
                         "Completed";

            if (status == "All")
            {
                return await _context.Orders
                    .Where(o => o.TimeCreate.Month == month && 
                                o.TimeCreate.Year == DateTime.Now.Year)
                    .ToListAsync();
            }
            else
            {
                return await _context.Orders
                    .Where(o => o.TimeCreate.Month == month &&
                                o.TimeCreate.Year == DateTime.Now.Year &&
                                o.Status == status)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetToday(int type)
        {
            var status = type == 0 ? "All" :
                         type == 1 ? "Pending" :
                         type == 2 ? "Packaging" :
                         type == 3 ? "Delivering" :
                         "Completed";

            if (status == "All")
            {
                return await _context.Orders
                    .Where(o => o.TimeCreate == DateTime.Now)
                    .ToListAsync();
            }
            else
            {
                return await _context.Orders
                    .Where(o => o.TimeCreate == DateTime.Now &&
                                o.Status == status)
                    .ToListAsync();
            }
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
