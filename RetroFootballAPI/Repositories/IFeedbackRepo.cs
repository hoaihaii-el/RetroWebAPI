using RetroFootballAPI.ViewModels;
using RetroFootballAPI.Models;

namespace RetroFootballAPI.Repositories
{
    public interface IFeedbackRepo
    {
        Task<IEnumerable<Feedback>> GetAll(string productID);
        Task<IEnumerable<Feedback>> GetAllByCustomerID(string productID);
        Task<double> GetAvgPoint(string productID);
        Task<Feedback> Add(FeedbackVM feedback);
        Task<Feedback> Update(FeedbackVM feedback);
    }
}
