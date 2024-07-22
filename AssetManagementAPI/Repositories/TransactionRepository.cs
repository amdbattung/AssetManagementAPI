using AssetManagementAPI.Data;
using AssetManagementAPI.DTO;
using AssetManagementAPI.Interfaces;
using AssetManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace AssetManagementAPI.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataContext _context;
        private bool _disposed;

        public TransactionRepository(DataContext context)
        {
            this._context = context;
            this._disposed = false;
        }

        public async Task<(ICollection<Transaction> Data, int PageNumber, int PageSize, int ItemCount)> GetAllAsync(QueryObject? queryObject)
        {
            var transaction = _context.Transactions.AsQueryable();

            transaction = transaction.OrderBy(t => EF.Property<Instant>(t, "DateCreated"));

            if (!string.IsNullOrWhiteSpace(queryObject?.Query))
            {
                transaction = transaction.Where(t => EF.Functions.ToTsVector("english", t.Reason + " " + t.Remark).Matches(EF.Functions.ToTsQuery("english", $"{queryObject.Query}:*")));
            }

            int pageNumber = queryObject?.PageNumber ?? 1;
            int pageSize = queryObject?.PageSize ?? 10;

            return (await transaction
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(t => t.Asset)
                .Include(t => t.Transactor)
                .Include(t => t.Transactee)
                .Include(t => t.Approver)
                .ToListAsync(),
                pageNumber,
                pageSize,
                await transaction.CountAsync());
        }

        public async Task<Transaction?> CreateAsync(CreateTransactionDTO transaction)
        {
            Asset? asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == transaction.AssetId);
            Employee? transactor = await _context.Employees.FirstOrDefaultAsync(e => e.Id == transaction.TransactorId);
            Employee? transactee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == transaction.TransacteeId);

            if (asset == null ||
                transactor == null ||
                transactee == null)
            {
                return null;
            }

            Transaction newTransaction = new()
            {
                #nullable disable
                Id = null,
                #nullable restore
                Asset = asset,
                Type = transaction.Type != null ? transaction.Type.Value : Transaction.TransactionType.Delegate,
                Transactor = transactor,
                Transactee = transactee,
                Date = transaction.Date,
                Reason = transaction.Reason,
                Remark = transaction.Remark,
                Approver = transaction.ApproverId != null ? await _context.Employees.FirstOrDefaultAsync(e => e.Id == transaction.ApproverId) : null
            };

            await _context.Transactions.AddAsync(newTransaction);
            return await _context.SaveChangesAsync() > 0 ? newTransaction : null;
        }

        public async Task<Transaction?> GetByIdAsync(string id)
        {
            return await _context.Transactions
                .Include(t => t.Asset)
                .Include(t => t.Transactor)
                .Include(t => t.Transactee)
                .Include(t => t.Approver)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Transaction?> UpdateAsync(string id, UpdateTransactionDTO transaction)
        {
            Transaction? existingTransaction = await _context.Transactions
                .Include(t => t.Asset)
                .Include(t => t.Transactor)
                .Include(t => t.Transactee)
                .Include(t => t.Approver)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTransaction == null)
            {
                return null;
            }

            existingTransaction.Asset = !string.IsNullOrWhiteSpace(transaction.AssetId) ? await _context.Assets.FirstOrDefaultAsync(a => a.Id == transaction.AssetId) ?? existingTransaction.Asset : existingTransaction.Asset;
            existingTransaction.Type = transaction.Type != null ? transaction.Type.Value : existingTransaction.Type;
            existingTransaction.Transactor = !string.IsNullOrWhiteSpace(transaction.TransactorId) ? await _context.Employees.FirstOrDefaultAsync(e => e.Id == transaction.TransactorId) ?? existingTransaction.Transactor : existingTransaction.Transactor;
            existingTransaction.Transactee = !string.IsNullOrWhiteSpace(transaction.TransacteeId) ? await _context.Employees.FirstOrDefaultAsync(e => e.Id == transaction.TransacteeId) ?? existingTransaction.Transactee : existingTransaction.Transactee;
            existingTransaction.Date = transaction.Date;
            existingTransaction.Reason = transaction.Reason;
            existingTransaction.Remark = transaction.Remark;
            existingTransaction.Approver = !string.IsNullOrWhiteSpace(transaction.ApproverId) ? await _context.Employees.FirstOrDefaultAsync(e => e.Id == transaction.ApproverId) : null;

            await _context.SaveChangesAsync();
            return existingTransaction;
        }

        public async Task<Transaction?> DeleteAsync(string id)
        {
            Transaction? transaction = await _context.Transactions
                .Include(t => t.Asset)
                .Include(t => t.Transactor)
                .Include(t => t.Transactee)
                .Include(t => t.Approver)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return null;
            }

            _context.Transactions.Remove(transaction);
            return await _context.SaveChangesAsync() > 0 ? transaction : null;
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
