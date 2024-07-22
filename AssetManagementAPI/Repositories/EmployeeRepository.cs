using AssetManagementAPI.Data;
using AssetManagementAPI.DTO;
using AssetManagementAPI.Interfaces;
using AssetManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace AssetManagementAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;
        private bool _disposed;

        public EmployeeRepository(DataContext context)
        {
            this._context = context;
            this._disposed = false;
        }

        public async Task<(ICollection<Employee> Data, int PageNumber, int PageSize, int ItemCount)> GetAllAsync(QueryObject? queryObject)
        {
            var employee = _context.Employees.AsQueryable();

            employee = employee.OrderBy(e => EF.Property<Instant>(e, "DateCreated"));

            if (!string.IsNullOrWhiteSpace(queryObject?.Query))
            {
                employee = employee.Where(e => EF.Functions.ToTsVector("simple", e.LastName + " " + e.FirstName + " " + e.MiddleName).Matches(EF.Functions.ToTsQuery("simple", $"{queryObject.Query}:*")));
            }

            int pageNumber = queryObject?.PageNumber ?? 1;
            int pageSize = queryObject?.PageSize ?? 10;

            return (await employee
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(e => e.Department)
                .ToListAsync(),
                pageNumber,
                pageSize,
                await employee.CountAsync());
        }

        public async Task<Employee?> CreateAsync(CreateEmployeeDTO employee)
        {
            Employee newEmployee = new()
            {
                #nullable disable
                Id = null,
                #nullable restore
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                Department = employee.DepartmentId != null ? await _context.Departments.FirstOrDefaultAsync(d => d.Id == employee.DepartmentId) : null
            };

            if (!string.IsNullOrWhiteSpace(employee.DepartmentId) && newEmployee.Department == null)
            {
                return null;
            }

            await _context.Employees.AddAsync(newEmployee);
            return await _context.SaveChangesAsync() > 0 ? newEmployee : null;
        }

        public async Task<Employee?> GetByIdAsync(string id)
        {
            return await _context.Employees.Where(e => e.Id == id).Include(e => e.Department).FirstOrDefaultAsync();
        }

        public async Task<Employee?> UpdateAsync(string id, UpdateEmployeeDTO employee)
        {
            Employee? existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (existingEmployee == null)
            {
                return null;
            }

            existingEmployee.LastName = employee.LastName;
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.MiddleName = employee.MiddleName;
            existingEmployee.Department = employee.DepartmentId != null ? await _context.Departments.FirstOrDefaultAsync(d => d.Id == employee.DepartmentId) : null;

            await _context.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task<Employee?> DeleteAsync(string id)
        {
            Employee? employee = await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return null;
            }

            _context.Employees.Remove(employee);
            return await _context.SaveChangesAsync() > 0 ? employee : null;
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
