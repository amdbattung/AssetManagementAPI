namespace AssetManagementAPI.Models
{
    public class MaintenanceRecord
    {
        public required string Id { get; set; }
        public required Asset Asset { get; set; }
        public required MaintenanceAction Action { get; set; }
        public Employee? Documentor { get; set; }
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
