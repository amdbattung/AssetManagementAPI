using AssetManagementAPI.DTO;
using AssetManagementAPI.Models;
using AssetManagementAPI.Services.Helpers;

namespace AssetManagementAPI.Interfaces
{
    public interface IMaintenanceRecordRepository : IDisposable
    {
        Task<(ICollection<MaintenanceRecord> Data, int PageNumber, int PageSize, int ItemCount)> GetAllAsync(QueryObject? queryObject);
        Task<MaintenanceRecord?> CreateAsync(CreateMaintenanceRecordDTO record);
        Task<MaintenanceRecord?> GetByIdAsync(string id);
        Task<MaintenanceRecord?> UpdateAsync(string id, UpdateMaintenanceRecordDTO record);
        Task<MaintenanceRecord?> DeleteAsync(string id);
    }
}
