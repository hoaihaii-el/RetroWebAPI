﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<Voucher> Add(VoucherVM model)
        {
            var voucher = new Voucher()
            {
                ID = await AutoID(),
                Name = model.Name,
                DateBegin = model.DateBegin,
                DateEnd = model.DateEnd,
                Value = model.Value
            };

            var customers = await _context.Customers.ToListAsync();

            foreach (var customer in customers)
            {
                _context.VoucherApplied.Add(new VoucherApplied
                {
                    VoucherID = voucher.ID,
                    CustomerID = customer.ID
                });
            }

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

        public async Task<IEnumerable<Voucher>> Filter(string param, string customerID)
        {
            var vouchers = await _context.VoucherApplied
                .Where(v => v.CustomerID == customerID)
                .Select(v => v.VoucherID) 
                .ToListAsync();

            switch (param)
            {
                case "New":
                    return await _context.Voucher
                        .Where(v => vouchers.Contains(v.ID) &&
                                    v.DateBegin <= DateTime.Now &&
                                    v.DateEnd >= DateTime.Now)
                        .OrderByDescending(v => v.DateEnd)
                        .ToListAsync();
                default: //almost expire
                    return await _context.Voucher
                        .Where(v => vouchers.Contains(v.ID) &&
                                    v.DateBegin <= DateTime.Now &&
                                    v.DateEnd >= DateTime.Now &&
                                    DateTime.Now.AddDays(3) >= v.DateEnd)
                        .OrderBy(v => v.DateEnd)
                        .ToListAsync();
            }
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

        public async Task<IEnumerable<Voucher>> SearchByName(string name)
        {
            return await _context.Voucher
                .Where(v => v.Name.Contains(name))
                .ToListAsync();
        }
    }
}
