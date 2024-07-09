using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NodaTime;
using AssetManagementAPI.DTO;

namespace AssetManagementAPI.Models
{
    public class MaintenanceRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public required string Id { get; set; }
        public required Asset Asset { get; set; }
        public required MaintenanceAction Action { get; set; }
        public Employee? Documentor { get; set; }
        [Column("maintenance_date")]
        public Instant? Date { get; set; }
        public string? Reason { get; set; }
        public string? Comment { get; set; }

        public GetMaintenanceRecordDTO ToDto()
        {
            return new GetMaintenanceRecordDTO(
                id: this.Id,
                assetId: this.Asset.Id,
                action: this.Action,
                documentorId: this.Documentor?.Id,
                date: this.Date,
                reason: this.Reason,
                comment: this.Comment
            );
        }

        public enum MaintenanceAction
        {
            Report,
            Service,
            Decommission
        }
    }
}
