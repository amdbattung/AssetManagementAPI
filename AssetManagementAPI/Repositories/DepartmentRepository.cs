using AssetManagementAPI.Data;
using AssetManagementAPI.DTO;
using AssetManagementAPI.Interfaces;
using AssetManagementAPI.Models;
using AssetManagementAPI.Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementAPI.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataContext _context;
        private bool _disposed;

        public DepartmentRepository(DataContext context)
        {
            this._context = context;
            this._disposed = false;
        }

        public async Task<(ICollection<Department> Data, int PageNumber, int PageSize, int ItemCount)> GetAllAsync(QueryObject? queryObject)
        {
            var department = _context.Departments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject?.Query))
            {
                department = department.Where(d => EF.Functions.ToTsVector("simple", d.Name).Matches(EF.Functions.ToTsQuery("simple", $"{queryObject.Query}:*")));
            }

            int pageNumber = queryObject?.PageNumber ?? 1;
            int pageSize = queryObject?.PageSize ?? 10;

            return (await department
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
                pageNumber,
                pageSize,
                await department.CountAsync());
        }

        public async Task<Department?> CreateAsync(CreateDepartmentDTO department)
        {
            Department newDepartment = new()
            {
                #nullable disable
                Id = null,
                #nullable restore
                Name = department.Name?.Trim() ?? ""
            };

            await _context.Departments.AddAsync(newDepartment);
            return await _context.SaveChangesAsync() > 0 ? newDepartment : null;
        }

        public async Task<Department?> GetByIdAsync(string id)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Department?> UpdateAsync(string id, UpdateDepartmentDTO department)
        {
            Department? existingDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (existingDepartment == null)
            {
                return null;
            }

            existingDepartment.Name = department.Name?.Trim() ?? existingDepartment.Name;

            await _context.SaveChangesAsync();
            return existingDepartment;
        }

        public async Task<Department?> DeleteAsync(string id)
        {
            Department? department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return null;
            }

            _context.Departments.Remove(department);
            return await _context.SaveChangesAsync() > 0 ? department : null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
