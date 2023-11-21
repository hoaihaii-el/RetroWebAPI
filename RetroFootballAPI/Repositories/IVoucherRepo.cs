using RetroFootballAPI.Models;

namespace RetroFootballAPI.Repositories
{
    public interface IVoucherRepo
    {
        Task<Voucher> Add(Voucher voucher);
        Task<Voucher> Update(Voucher voucher);
        Task<Voucher> Delete(string voucherID);
        Task<IEnumerable<Voucher>> GetAll();
        Task<IEnumerable<Voucher>> GetAvailable();
        Task<Voucher> GetById(string voucherID);
        Task<Voucher> GetVoucherApplied(string productID);
    }
}
