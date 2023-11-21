using RetroFootballAPI.Models;
using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Repositories
{
    public interface IDeliveryInfoRepo
    {
        Task<IEnumerable<DeliveryInfo>> GetAll(string customerID);
        Task<DeliveryInfo> GetByID(string customerID, int priority);
        Task<DeliveryInfo> SetDefault(string customerID, int priority);
        Task<DeliveryInfo> Add(DeliveryInfoVM info);
        Task<DeliveryInfo> Update(DeliveryInfoVM info);
        Task<DeliveryInfo> Delete(string customerID, int priority);
    }
}
