namespace AssetManagementAPI.Models
{
    public class Transaction
    {
        public required string Id { get; set; }
        public required Asset Asset { get; set; }
        public required TransactionType Type { get; set; }
        public required Employee Transactor { get; set; }
        public required Employee Transactee { get; set; }
        public DateTime? Date { get; set; }
        public string? Reason { get; set; }
        public string? Remark { get; set; }
        public Employee? Approver { get; set; }

        public enum TransactionType
        {
            Delegate,
            Transfer,
            Return,
            HandOver
        }
    }
}
