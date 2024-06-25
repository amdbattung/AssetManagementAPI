using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NodaTime;

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

        public enum MaintenanceAction
        {
            Report,
            Service,
            Decommission
        }
    }
}
