using NodaTime;
using System.Text.Json.Serialization;
using static AssetManagementAPI.Models.Transaction;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class GetTransactionDTO
    {
        public string? Id { get; }
        public string? AssetId { get; }
        public TransactionType? Type { get; }
        public string? TransactorId { get; }
        public string? TransacteeId { get; }
        public Instant? Date { get; }
        public string? Reason { get; }
        public string? Remark { get; }
        public string? ApproverId { get; }

        public GetTransactionDTO(string? id, string? assetId, TransactionType? type, string? transactorId, string? transacteeId, Instant? date, string? reason, string? remark, string? approverId)
        {
            this.Id = id;
            this.AssetId = assetId;
            this.Type = type;
            this.TransactorId = transactorId;
            this.TransacteeId = transacteeId;
            this.Date = date;
            this.Reason = reason;
            this.Remark = remark;
            this.ApproverId = approverId;
        }
    }
}
