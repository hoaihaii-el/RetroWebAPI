using RetroFootballAPI.ViewModels;
using RetroFootballAPI.Models;

namespace RetroFootballAPI.Repositories
{
    public interface IOrderRepo
    {
        Task<IEnumerable<Order>> GetToday(int type);
        Task<IEnumerable<Order>> GetByMonth(int month, int type);
        Task<IEnumerable<Order>> GetByCustomer(string customerID, int type);
        Task<Order> Add(OrderVM order);
        Task<Order> UpdateStatus(int orderID);
        Task<Order> Delete(int orderID);
    }
}
