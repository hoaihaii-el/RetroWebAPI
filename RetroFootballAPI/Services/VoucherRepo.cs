using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RetroFootballAPI.Models;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;
using RetroFootballWeb.Repository;
using System.Text.RegularExpressions;

namespace RetroFootballAPI.Services
{
    public class VoucherRepo : IVoucherRepo
    {
        private readonly DataContext _context;

        public VoucherRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<Voucher> Add(VoucherVM model, List<string> productsApplied)
        {
            var voucher = new Voucher()
            {
                ID = await AutoID(),
                Name = model.Name,
                DateBegin = model.DateBegin,
                DateEnd = model.DateEnd,
                Value = model.Value
            };

            _context.Voucher.Add(voucher);
            await _context.SaveChangesAsync();

            foreach (var productID in productsApplied)
            {
                _context.VoucherApplied.Add(new VoucherApplied
                {
                    VoucherID = voucher.ID,
                    ProductID = productID
                });
            }

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
                .Select(v => v.VoucherID)
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

        private async Task<string> AutoID()
        {
            var ID = "VC0001";

            var maxID = await _context.Voucher
                .OrderByDescending(v => v.ID)
                .Select(v => v.ID)
                .FirstOrDefaultAsync();

            if (maxID == null)
            {
                return ID;
            }

            ID = "VC";

            var numeric = Regex.Match(maxID, @"\d+").Value;

            numeric = (int.Parse(numeric) + 1).ToString();

            while (ID.Length + numeric.Length < 6)
            {
                ID += '0';
            }

            return ID + numeric;
        }
    }
}
