using Microsoft.EntityFrameworkCore;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballWeb.Repository;

namespace RetroFootballAPI.Services
{
    public class VoucherRepo : IVoucherRepo
    {
        private readonly DataContext _context;

        public VoucherRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<Voucher> Add(Voucher voucher)
        {
            _context.Voucher.Add(voucher);
            await _context.SaveChangesAsync();

            return voucher;
        }

        public async Task<Voucher> Delete(string voucherID)
        {
            var voucher = await _context.Voucher.FindAsync(voucherID);

            if (voucher == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Voucher.Remove(voucher);

            await _context.SaveChangesAsync();

            return voucher;
        }

        public async Task<IEnumerable<Voucher>> GetAll()
        {
            return await _context.Voucher.ToListAsync();
        }

        public async Task<IEnumerable<Voucher>> GetAvailable()
        {
            return await _context.Voucher
                .Where(v => v.DateBegin <= DateTime.Now && 
                            v.DateEnd >= DateTime.Now)
                .ToListAsync();
        }

        public async Task<Voucher> GetById(string voucherID)
        {
            var voucher = await _context.Voucher.FindAsync(voucherID);

            if (voucher == null)
            {
                throw new KeyNotFoundException();
            }

            return voucher;
        }

        public async Task<Voucher> GetVoucherApplied(string productID)
        {
            var vouchers = await _context.VoucherApplied
                .Where(v => v.ProductID == productID)
                .ToListAsync();

            var maxVoucher = new Voucher();

            foreach (var voucherID in vouchers)
            {
                var voucher = await _context.Voucher.FindAsync(voucherID);

                if (voucher == null)
                {
                    continue;
                }

                if (voucher.Value > maxVoucher.Value)
                {
                    maxVoucher = voucher;
                }
            }

            return maxVoucher;
        }

        public async Task<Voucher> Update(Voucher voucher)
        {
            _context.Voucher.Update(voucher);
            await _context.SaveChangesAsync();

            return voucher;
        }
    }
}
