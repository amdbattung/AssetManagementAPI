using NodaTime;
using System.Text.Json.Serialization;
using static AssetManagementAPI.Models.Transaction;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class UpdateTransactionDTO
    {
        public string? AssetId { get; set; }
        public TransactionType? Type { get; set; }
        public string? TransactorId { get; set; }
        public string? TransacteeId { get; set; }
        public Instant? Date { get; set; }
        public string? Reason { get; set; }
        public string? Remark { get; set; }
        public string? ApproverId { get; set; }
    }
}