using AssetManagementAPI.DTO;
using AssetManagementAPI.Models;

namespace AssetManagementAPI.Interfaces
{
    public interface IAssetRepository : IDisposable
    {
        Task<(ICollection<Asset> Data, int PageNumber, int PageSize, int ItemCount)> GetAllAsync(QueryObject? queryObject);
        Task<Asset?> CreateAsync(CreateAssetDTO asset);
        Task<Asset?> GetByIdAsync(string id);
        Task<Asset?> UpdateAsync(string id, UpdateAssetDTO asset);
        Task<Asset?> DeleteAsync(string id);
    }
}
