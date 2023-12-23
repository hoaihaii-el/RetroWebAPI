using RetroFootballAPI.Models;
using RetroFootballAPI.ViewModels;

namespace RetroFootballAPI.Repositories
{
    public interface IVoucherRepo
    {
        Task<Voucher> Add(VoucherVM voucher, List<string> productsApplied);
        Task<Voucher> Update(Voucher voucher);
        Task<Voucher> Delete(string voucherID);
        Task<IEnumerable<Voucher>> GetAll();
        Task<IEnumerable<Voucher>> Filter(string param);
        Task<Voucher> GetById(string voucherID);
        Task<Voucher> GetVoucherApplied(string productID);
        Task<IEnumerable<Voucher>> SearchByName(string name);
    }
}
