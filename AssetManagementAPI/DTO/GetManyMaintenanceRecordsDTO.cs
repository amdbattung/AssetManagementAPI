using System.Text.Json.Serialization;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class GetManyMaintenanceRecordsDTO
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public int ItemCount { get; }
        [JsonPropertyName("data")]
        public IEnumerable<GetMaintenanceRecordDTO> MaintenanceRecords { get; }

        public GetManyMaintenanceRecordsDTO(int pageNumber, int pageSize, int itemCount, IEnumerable<GetMaintenanceRecordDTO> maintenanceRecords)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.ItemCount = itemCount;
            this.MaintenanceRecords = maintenanceRecords;
        }
    }
}
