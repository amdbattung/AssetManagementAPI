using AssetManagementAPI.DTO;
using AssetManagementAPI.Models;

namespace AssetManagementAPI.Interfaces
{
    public interface IDepartmentRepository : IDisposable
    {
        Task<(ICollection<Department> Data, int PageNumber, int PageSize, int ItemCount)> GetAllAsync(QueryObject? queryObject);
        Task<Department?> CreateAsync(CreateDepartmentDTO department);
        Task<Department?> GetByIdAsync(string id);
        Task<Department?> UpdateAsync(string id, UpdateDepartmentDTO department);
        Task<Department?> DeleteAsync(string id);
    }
}
