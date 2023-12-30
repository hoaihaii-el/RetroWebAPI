using RetroFootballAPI.Models;
using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Repositories
{
    public interface ICustomerRepo
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> GetByID(string id);
        Task<Customer> Add(Customer customer);
        Task<Customer> Update(CustomerVM customer);
        Task<Customer> Delete(string id);
        Task<IEnumerable<Customer>> SearchByName(string name);
    }
}
