using AssetManagementAPI.Data;
using AssetManagementAPI.DTO;
using AssetManagementAPI.Interfaces;
using AssetManagementAPI.Models;
using AssetManagementAPI.Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;

        public EmployeeRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<ICollection<Employee>> GetAllAsync(QueryObject? queryObject)
        {
            var employee = _context.Employees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject?.Query))
            {
                employee = employee.Where(e => EF.Functions.ToTsVector("simple", e.LastName + " " + e.FirstName + " " + e.MiddleName).Matches(EF.Functions.PlainToTsQuery(queryObject.Query)));
            }

            int pageNumber = queryObject?.PageNumber ?? 1;
            int pageSize = queryObject?.PageSize ?? 10;

            return await employee
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(e => e.Department)
                .ToListAsync();
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
            return await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();
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
            existingEmployee.Department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == employee.DepartmentId);

            return await _context.SaveChangesAsync() > 0 ? existingEmployee : null;
        }

        public async Task<Employee?> DeleteAsync(string id)
        {
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return null;
            }

            _context.Employees.Remove(employee);
            return await _context.SaveChangesAsync() > 0 ? employee : null;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
