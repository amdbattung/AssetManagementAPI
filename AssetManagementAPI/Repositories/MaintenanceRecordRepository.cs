using AssetManagementAPI.Data;
using AssetManagementAPI.DTO;
using AssetManagementAPI.Interfaces;
using AssetManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace AssetManagementAPI.Repositories
{
    public class MaintenanceRecordRepository : IMaintenanceRecordRepository
    {
        private readonly DataContext _context;
        private bool _disposed;

        public MaintenanceRecordRepository(DataContext context)
        {
            this._context = context;
            this._disposed = false;
        }

        public async Task<(ICollection<MaintenanceRecord> Data, int PageNumber, int PageSize, int ItemCount)> GetAllAsync(QueryObject? queryObject)
        {
            var record = _context.MaintenanceRecords.AsQueryable();

            record = record.OrderBy(r => EF.Property<Instant>(r, "DateCreated"));

            if (!string.IsNullOrWhiteSpace(queryObject?.Query))
            {
                record = record.Where(r => EF.Functions.ToTsVector("english", r.Reason + " " + r.Comment).Matches(EF.Functions.ToTsQuery("english", $"{queryObject.Query}:*")));
            }

            int pageNumber = queryObject?.PageNumber ?? 1;
            int pageSize = queryObject?.PageSize ?? 10;

            return (await record
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(r => r.Asset)
                .Include(r => r.Documentor)
                .ToListAsync(),
                pageNumber,
                pageSize,
                await record.CountAsync());
        }

        public async Task<MaintenanceRecord?> CreateAsync(CreateMaintenanceRecordDTO record)
        {
            Asset? asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == record.AssetId);

            if (asset == null)
            {
                return null;
            }

            MaintenanceRecord newRecord = new()
            {
                #nullable disable
                Id = null,
                #nullable restore
                Asset = asset,
                Action = record.Action != null ? record.Action.Value : MaintenanceRecord.MaintenanceAction.Report,
                Documentor = record.DocumentorId != null ? await _context.Employees.FirstOrDefaultAsync(e => e.Id == record.DocumentorId) : null,
                Date = record.Date,
                Reason = record.Reason,
                Comment = record.Comment,
            };

            await _context.MaintenanceRecords.AddAsync(newRecord);
            return await _context.SaveChangesAsync() > 0 ? newRecord : null;
        }

        public async Task<MaintenanceRecord?> GetByIdAsync(string id)
        {
            return await _context.MaintenanceRecords
                .Include(r => r.Asset)
                .Include(r => r.Documentor)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<MaintenanceRecord?> UpdateAsync(string id, UpdateMaintenanceRecordDTO record)
        {
            MaintenanceRecord? existingRecord = await _context.MaintenanceRecords
                .Include(r => r.Asset)
                .Include(r => r.Documentor)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (existingRecord == null)
            {
                return null;
            }

            existingRecord.Asset = !string.IsNullOrWhiteSpace(record.AssetId) ? await _context.Assets.FirstOrDefaultAsync(a => a.Id == record.AssetId) ?? existingRecord.Asset : existingRecord.Asset;
            existingRecord.Action = record.Action != null ? record.Action.Value : existingRecord.Action;
            existingRecord.Documentor = !string.IsNullOrWhiteSpace(record.DocumentorId) ? await _context.Employees.FirstOrDefaultAsync(e => e.Id == record.DocumentorId) : null;
            existingRecord.Date = record.Date;
            existingRecord.Reason = record.Reason;
            existingRecord.Comment = record.Comment;

            await _context.SaveChangesAsync();
            return existingRecord;
        }
        public async Task<MaintenanceRecord?> DeleteAsync(string id)
        {
            MaintenanceRecord? record = await _context.MaintenanceRecords
                .Include(r => r.Asset)
                .Include(r => r.Documentor)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (record == null)
            {
                return null;
            }

            _context.MaintenanceRecords.Remove(record);
            return await _context.SaveChangesAsync() > 0 ? record : null;
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
