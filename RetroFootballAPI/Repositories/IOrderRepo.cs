using RetroFootballAPI.ViewModels;
using RetroFootballAPI.Models;

namespace RetroFootballAPI.Repositories
{
    public interface IOrderRepo
    {
        Task<IEnumerable<Order>> GetOrders(
            int orderType = 0,
            int month = 0,
            string customerID = "",
            bool today = false);
        Task<IEnumerable<Order>> GetByCustomer(string customerID, int orderType);
        Task<Order> Add(OrderVM order);
        Task<Order> UpdateStatus(int orderID);
        Task<Order> UpdatePaymentStatus(int orderID);
        Task<Order> Cancel(int orderID, bool isCancelByAdmin);
    }
}
