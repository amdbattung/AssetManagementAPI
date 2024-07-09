using NodaTime;
using static AssetManagementAPI.Models.MaintenanceRecord;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class GetMaintenanceRecordDTO
    {
        public string? Id { get; }
        public string? AssetId { get; }
        public MaintenanceAction? Action { get; }
        public string? DocumentorId { get; }
        public Instant? Date { get; }
        public string? Reason { get; }
        public string? Comment { get; }

        public GetMaintenanceRecordDTO(string? id, string? assetId, MaintenanceAction? action, string? documentorId, Instant? date, string? reason, string? comment)
        {
            this.Id = id;
            this.AssetId = assetId;
            this.Action = action;
            this.DocumentorId = documentorId;
            this.Date = date;
            this.Reason = reason;
            this.Comment = comment;
        }
    }
}
