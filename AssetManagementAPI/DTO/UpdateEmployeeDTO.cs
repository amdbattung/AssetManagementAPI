namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class UpdateEmployeeDTO
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? DepartmentId { get; set; }
    }
}
