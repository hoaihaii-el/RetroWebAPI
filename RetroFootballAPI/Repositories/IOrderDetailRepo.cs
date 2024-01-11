using RetroFootballAPI.ViewModels;
using RetroFootballAPI.Models;

namespace RetroFootballAPI.Repositories
{
    public interface IOrderDetailRepo
    {
        Task<OrderDetailsGetVM> GetByOrderID(int orderID);
        Task<List<OrderProductVM>> getUnReviewedProducts(string customerID);
        Task<OrderDetail> Add(OrderDetailVM detail);
        Task<OrderDetail> Delete(string customerID, string productID, string size);
    }
}
