using static AssetManagementAPI.Models.Transaction;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class GetTransactionDTO
    {
        public string? Id { get; }
        public string? AssetId { get; }
        public TransactionType Type { get; }
        public string? TransactorId { get; }
        public string? TransacteeId { get; }
        public DateTime? Date { get; }
        public string? Reason { get; }
        public string? Remark { get; }
        public string? ApproverId { get; }
    }
}
