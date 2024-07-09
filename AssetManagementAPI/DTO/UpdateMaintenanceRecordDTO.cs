using NodaTime;
using static AssetManagementAPI.Models.MaintenanceRecord;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class UpdateMaintenanceRecordDTO
    {
        public string? AssetId { get; set; }
        public MaintenanceAction? Action { get; set; }
        public string? DocumentorId { get; set; }
        public Instant? Date { get; set; }
        public string? Reason { get; set; }
        public string? Comment { get; set; }
    }
}
