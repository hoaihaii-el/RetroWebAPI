using RetroFootballAPI.Responses;

namespace RetroFootballAPI.Repositories
{
    public interface IStatRepo
    {
        Task<RevenueResponse> GetRevenue(int param = 0);
        Task<List<decimal>> RevenueByMonths();
    }
}
