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

        public async Task<Customer> UpdateAvatar(UpdateAvatarVM avatar)
        {
            var user = await _context.Users.Where(u => u.Email == avatar.Email).FirstOrDefaultAsync();

            var customer = await _context.Customers.FindAsync(user?.Id);

            if (customer == null)
            {
                throw new KeyNotFoundException();
            }

            customer.Avatar = await UploadImage.Instance.UploadAsync(avatar.Avatar);

            _context.Customers.Update(customer);

            return customer;
        }
    }
}
