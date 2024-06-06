using AssetManagementAPI.Data;
using AssetManagementAPI.DTO;
using AssetManagementAPI.Interfaces;
using AssetManagementAPI.Models;
using AssetManagementAPI.Services.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementAPI.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly DataContext _context;

        public AssetRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<ICollection<Asset>> GetAllAsync(QueryObject? queryObject)
        {
            var asset = _context.Assets.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject?.Query))
            {
                asset = asset.Where(a => EF.Functions.ToTsVector("simple", a.Type + " " + a.Name).Matches(EF.Functions.PlainToTsQuery(queryObject.Query)));
            }

            int pageNumber = queryObject?.PageNumber ?? 1;
            int pageSize = queryObject?.PageSize ?? 10;

            return await asset
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Asset?> CreateAsync(CreateAssetDTO asset)
        {
            Asset newAsset = new()
            {
                #nullable disable
                Id = null,
                #nullable restore
                Type = asset.Type,
                Name = asset.Name ?? "",
                Info = asset.Info,
                Proprietor = asset.ProprietorId != null ? await _context.Departments.FirstOrDefaultAsync(d => d.Id == asset.ProprietorId) : null,
                Custodian = asset.CustodianId != null ? await _context.Employees.FirstOrDefaultAsync(e => e.Id == asset.CustodianId) : null,
                IsActive = true
            };

            if (!string.IsNullOrWhiteSpace(asset.ProprietorId) && newAsset.Proprietor == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(asset.CustodianId) && newAsset.Custodian == null)
            {
                return null;
            }

            await _context.Assets.AddAsync(newAsset);
            return await _context.SaveChangesAsync() > 0 ? newAsset : null;
        }

        public async Task<Asset?> GetByIdAsync(string id)
        {
            return await _context.Assets.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Asset?> UpdateAsync(string id, UpdateAssetDTO asset)
        {
            Asset? existingAsset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);

            if (existingAsset == null)
            {
                return null;
            }

            existingAsset.Type = asset.Type;
            existingAsset.Name = asset.Name ?? existingAsset.Name;
            existingAsset.Info = asset.Info;
            existingAsset.Proprietor = await _context.Departments.FirstOrDefaultAsync(d => d.Id == asset.ProprietorId);
            existingAsset.Custodian = await _context.Employees.FirstOrDefaultAsync(e => e.Id == asset.CustodianId);
            existingAsset.IsActive = asset.IsActive ?? existingAsset.IsActive;

            return await _context.SaveChangesAsync() > 0 ? existingAsset : null;
        }

        public async Task<Asset?> DeleteAsync(string id)
        {
            Asset? asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);

            if (asset == null)
            {
                return null;
            }

            _context.Assets.Remove(asset);
            return await _context.SaveChangesAsync() > 0 ? asset : null;
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
