using AssetManagementAPI.DTO;
using AssetManagementAPI.Models;

namespace AssetManagementAPI.Interfaces
{
    public interface ITransactionRepository : IDisposable
    {
        Task<(ICollection<Transaction> Data, int PageNumber, int PageSize, int ItemCount)> GetAllAsync(QueryObject? queryObject);
        Task<Transaction?> CreateAsync(CreateTransactionDTO employee);
        Task<Transaction?> GetByIdAsync(string id);
        Task<Transaction?> UpdateAsync(string id, UpdateTransactionDTO employee);
        Task<Transaction?> DeleteAsync(string id);
    }
}
