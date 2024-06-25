using System.Text.Json.Serialization;
using static AssetManagementAPI.Models.MaintenanceRecord;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class GetMaintenanceRecordDTO
    {
        public string? Id { get; }
        public string? AssetId { get; }
        public MaintenanceAction? Action { get; }
        public string? Documentor { get; }
        public string? Reason { get; }
        public string? Comment { get; }
    }
}
