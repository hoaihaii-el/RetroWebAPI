using RetroFootballAPI.ViewModels;
using RetroFootballAPI.Models;

namespace RetroFootballAPI.Repositories
{
    public interface IOrderDetailRepo
    {
        Task<IEnumerable<OrderDetail>> GetByOrderID(int orderID);
        Task<OrderDetail> Add(OrderDetailVM detail);
        Task<OrderDetail> Delete(string customerID, string productID, string size);
    }
}
