using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;
using RetroFootballWeb.Repository;

namespace RetroFootballAPI.Services
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly DataContext _context;

        public CategoryRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryVM>> GetClubs()
        {
            return await _context.Products
                .Where(p => p.Club != "" && p.Club != "None")
                .Select(p => new CategoryVM
                {
                    Value = p.Club,
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryVM>> GetNations()
        {
            return await _context.Products
                .Where(p => p.Nation != "" && p.Nation != "None")
                .Select(p => new CategoryVM
                {
                    Value = p.Nation,
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<CategoryVM>> GetSeasons()
        {
            return await _context.Products
                .Where(p => p.Season != "None")
                .Select(p => new CategoryVM
                {
                    Value = p.Season,
                })
                .Distinct()
                .ToListAsync();
        }
    }
}
