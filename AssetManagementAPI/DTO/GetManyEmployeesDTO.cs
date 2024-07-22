using System.Text.Json.Serialization;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class GetManyEmployeesDTO
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public int ItemCount { get; }
        [JsonPropertyName("data")]
        public IEnumerable<GetEmployeeDTO> Employees { get; }

        public GetManyEmployeesDTO(int pageNumber, int pageSize, int itemCount, IEnumerable<GetEmployeeDTO> employees)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.ItemCount = itemCount;
            this.Employees = employees;
        }
    }
}
