using AssetManagementAPI.DTO;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagementAPI.Models
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public required string Id { get; set; }
        public required Asset Asset { get; set; }
        public required TransactionType Type { get; set; }
        public required Employee Transactor { get; set; }
        public required Employee Transactee { get; set; }
        [Column("transaction_date")]
        public Instant? Date { get; set; }
        public string? Reason { get; set; }
        public string? Remark { get; set; }
        public Employee? Approver { get; set; }

        public GetTransactionDTO ToDto()
        {
            return new GetTransactionDTO(
                id: this.Id,
                assetId: this.Asset.Id,
                type: this.Type,
                transactorId: this.Transactor.Id,
                transacteeId: this.Transactee.Id,
                date: this.Date,
                reason: this.Reason,
                remark: this.Remark,
                approverId: this.Approver?.Id
            );
        }

        public enum TransactionType
        {
            Delegate,
            Transfer,
            Return,
            HandOver
        }
    }
}
