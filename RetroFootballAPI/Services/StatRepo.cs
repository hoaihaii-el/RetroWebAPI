using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.Responses;
using RetroFootballWeb.Repository;
using SQLitePCL;

namespace RetroFootballAPI.Services
{
    public class StatRepo : IStatRepo
    {
        private readonly DataContext _context;

        public StatRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<RevenueResponse> GetRevenue(int param = 0)
        {
            decimal revenue = 0, last = 0;

            switch (param)
            {
                case 0: //today
                    revenue = await _context.Orders
                        .Where(o => o.TimeCreate.Date == DateTime.Now.Date)
                        .Select(o => o.Value)
                        .SumAsync();

                    last = await _context.Orders
                        .Where(o => o.TimeCreate.Date.AddDays(1) == DateTime.Now.Date)
                        .Select(o => o.Value)
                        .SumAsync();
                    break;
                case 1: //this month
                    revenue = await _context.Orders
                        .Where(o => o.TimeCreate.Month == DateTime.Now.Month &&
                                    o.TimeCreate.Year == DateTime.Now.Year)
                        .Select(o => o.Value)
                        .SumAsync();

                    last = await _context.Orders
                        .Where(o => o.TimeCreate.AddMonths(1).Month == DateTime.Now.Month)
                        .Select(o => o.Value)
                        .SumAsync();
                    break;
                default: //this year
                    revenue = await _context.Orders
                        .Where(o => o.TimeCreate.Year == DateTime.Now.Year)
                        .Select(o => o.Value)
                        .SumAsync();

                    last = await _context.Orders
                        .Where(o => o.TimeCreate.Year + 1 == DateTime.Now.Year)
                        .Select(o => o.Value)
                        .SumAsync();
                    break;
            }

            var stat = new RevenueResponse
            {
                Revenue = revenue,
            };

            if (last != 0)
            {
                stat.Percent = Math.Round((revenue - last) * 100 / last, 2);
            }
            else
            {
                stat.Percent = 0;
            }

            return stat;
        }

        public async Task<List<decimal>> RevenueByMonths()
        {
            var revenue = new List<decimal>();

            for (int i = 0; i < DateTime.Now.Month; i++)
            {
                revenue.Add(await _context.Orders
                    .Where(o => o.TimeCreate.Month == i &&
                                o.TimeCreate.Year == DateTime.Now.Year)
                    .Select(o => o.Value)
                    .SumAsync());
            }

            return revenue;
        }
    }
}
