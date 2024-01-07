using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.StaticService;
using RetroFootballAPI.ViewModels;
using RetroFootballWeb.Repository;

namespace RetroFootballAPI.Services
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly DataContext _context;

        public CustomerRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<Customer> Add(Customer customer)
        {
            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            return customer;
        }


        public async Task<Customer> Update(CustomerVM customerVM)
        {
            var customer = await _context.Customers.FindAsync(customerVM.ID);

            if (customer == null)
            {
                throw new KeyNotFoundException();
            }

            customer.Name = customerVM.Name;
            customer.Address = customerVM.Address;
            customer.DateBirth = customerVM.DateBirth;
            customer.Phone = customerVM.Phone;
            if(customerVM.Avatar != "")
            {
                customer.Avatar = await UploadImage.Instance.UploadAsync(customer.ID + Guid.NewGuid().ToString(), customerVM.Avatar);
            }

            _context.Customers.Update(customer);

            await _context.SaveChangesAsync();

            return customer;
        }


        public async Task<Customer> Delete(string id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                throw new KeyNotFoundException(id);
            }

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return customer;
        }


        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _context.Customers
                .Where(c => c.ID.ToLower().IndexOf("cs") == 0)
                .ToListAsync();
        }


        public async Task<Customer> GetByID(string id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                throw new KeyNotFoundException(id);
            }

            return customer;
        }

        public async Task<IEnumerable<Customer>> SearchByName(string name)
        {
            return await _context.Customers
                .Where(c => c.Name.Contains(name))
                .ToListAsync();
        }
    }
}
