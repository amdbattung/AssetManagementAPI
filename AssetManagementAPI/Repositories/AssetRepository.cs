using AssetManagementAPI.Data;
using AssetManagementAPI.DTO;
using AssetManagementAPI.Interfaces;
using AssetManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json;

namespace AssetManagementAPI.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly DataContext _context;
        private bool _disposed;

        public AssetRepository(DataContext context)
        {
            this._context = context;
            this._disposed = false;
        }

        public async Task<(ICollection<Asset> Data, int PageNumber, int PageSize, int ItemCount)> GetAllAsync(QueryObject? queryObject)
        {
            var asset = _context.Assets.AsQueryable();

            asset = asset.OrderBy(a => EF.Property<Instant>(a, "DateCreated"));

            if (!string.IsNullOrWhiteSpace(queryObject?.Query))
            {
                asset = asset.Where(a => EF.Functions.ToTsVector("simple", a.Type + " " + a.Name).Matches(EF.Functions.ToTsQuery("simple", $"{queryObject.Query}:*")));
            }

            int pageNumber = queryObject?.PageNumber ?? 1;
            int pageSize = queryObject?.PageSize ?? 10;

            return (await asset
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
                pageNumber,
                pageSize,
                await asset.CountAsync());
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
                Info = asset.Info != null ? JsonDocument.Parse(((JsonElement)asset.Info).GetRawText()) : null,
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
            using Asset? asset = await _context.Assets.Where(a => a.Id == id).FirstOrDefaultAsync();
            return asset;
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
            existingAsset.Info = asset.Info != null ? JsonDocument.Parse(((JsonElement)asset.Info).GetRawText()) : null;
            existingAsset.Proprietor = asset.ProprietorId != null ? await _context.Departments.FirstOrDefaultAsync(d => d.Id == asset.ProprietorId) : null;
            existingAsset.Custodian = asset.CustodianId != null ? await _context.Employees.FirstOrDefaultAsync(e => e.Id == asset.CustodianId) : null;
            existingAsset.IsActive = asset.IsActive ?? existingAsset.IsActive;

            await _context.SaveChangesAsync();
            return existingAsset;
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
