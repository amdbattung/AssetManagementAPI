using AssetManagementAPI.DTO;
using AssetManagementAPI.Models;
using AssetManagementAPI.Services.Helpers;

namespace AssetManagementAPI.Interfaces
{
    public interface IAssetRepository : IDisposable
    {
        Task<ICollection<Asset>> GetAllAsync(QueryObject? queryObject);
        Task<Asset?> CreateAsync(CreateAssetDTO asset);
        Task<Asset?> GetByIdAsync(string id);
        Task<Asset?> UpdateAsync(string id, UpdateAssetDTO asset);
        Task<Asset?> DeleteAsync(string id);
    }
}
