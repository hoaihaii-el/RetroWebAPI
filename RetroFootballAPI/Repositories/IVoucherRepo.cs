using RetroFootballAPI.Models;
using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Repositories
{
    public interface IVoucherRepo
    {
        Task<Voucher> Add(VoucherVM voucher);
        Task<Voucher> Update(Voucher voucher);
        Task<Voucher> Delete(string voucherID);
        Task<IEnumerable<Voucher>> GetAll();
        Task<IEnumerable<Voucher>> Filter(string param, string customerID);
        Task<Voucher> GetById(string voucherID);
        Task<IEnumerable<Voucher>> SearchByName(string name);
    }
}
