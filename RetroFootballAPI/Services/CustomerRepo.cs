using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
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


        public async Task<Customer> Update(Customer customer)
        {
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
            return await _context.Customers.ToListAsync();
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
    }
}
