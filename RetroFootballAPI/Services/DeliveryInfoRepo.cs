using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;
using RetroFootballWeb.Repository;

namespace RetroFootballAPI.Services
{
    public class DeliveryInfoRepo : IDeliveryInfoRepo
    {
        private readonly DataContext _context;

        public DeliveryInfoRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<DeliveryInfo> Add(DeliveryInfoVM info)
        {
            var priority = await _context.DeliveryInfos
            .Where(x => x.CustomerID == info.CustomerID)
            .Select(x => (int?)x.Priority)
            .MaxAsync() + 1 ?? 0;

            if (priority < 1) priority = 1;

            var deliInfo = new DeliveryInfo
            {
                CustomerID = info.CustomerID,
                Priority = priority,
                Name = info.Name,
                Address = info.Address,
                Phone = info.Phone
            };

            var customer = await _context.Customers.FindAsync(info.CustomerID);

            if (customer == null)
            {
                throw new KeyNotFoundException();
            }

            deliInfo.Customer = customer;

            _context.DeliveryInfos.Add(deliInfo);
            await _context.SaveChangesAsync();

            return deliInfo;
        }

        public async Task<DeliveryInfo> Delete(string customerID, int priority)
        {
            var info = await _context.DeliveryInfos
                .FindAsync(customerID, priority);

            if (info == null)
            {
                throw new KeyNotFoundException();
            }

            _context.DeliveryInfos.Remove(info);

            await _context.SaveChangesAsync();

            return info;
        }

        public async Task<IEnumerable<DeliveryInfo>> GetAll(string customerID)
        {
            return await _context.DeliveryInfos
                .Where(i => i.CustomerID == customerID)
                .ToListAsync();
        }

        public async Task<DeliveryInfo> GetByID(string customerID, int priority)
        {
            var info = await _context.DeliveryInfos.FindAsync(customerID, priority);

            if (info == null)
            {
                throw new KeyNotFoundException();
            }

            return info;
        }

        public async Task<DeliveryInfo> SetDefault(string customerID, int priority)
        {
            var info = await _context.DeliveryInfos.FindAsync(customerID, priority);
            var defaultInfo = await _context.DeliveryInfos.FindAsync(customerID, 1);

            if (info == null || defaultInfo == null)
            {
                throw new KeyNotFoundException();
            }

            if (info.Priority == 1)
            {
                return info;
            }

            var temp = defaultInfo.Name;
            defaultInfo.Name = info.Name;
            info.Name = temp;

            temp = defaultInfo.Address;
            defaultInfo.Address= info.Address;
            info.Address = temp;

            temp = defaultInfo.Phone;
            defaultInfo.Phone = info.Phone;
            info.Phone = temp;

            await _context.SaveChangesAsync();

            return defaultInfo;
        }

        public async Task<DeliveryInfo> Update(DeliveryInfoVM infoVM, int pri)
        {
            var info = await _context.DeliveryInfos
                .FindAsync(infoVM.CustomerID, pri);

            if (info == null)
            {
                throw new KeyNotFoundException();
            }

            info.Name = infoVM.Name;
            info.Address = infoVM.Address;
            info.Phone = infoVM.Phone;

            _context.DeliveryInfos.Update(info);

            await _context.SaveChangesAsync();

            return info;
        }
    }
}
