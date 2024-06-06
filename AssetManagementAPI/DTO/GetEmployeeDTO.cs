namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class GetEmployeeDTO
    {
        public string? Id { get; }
        public string? LastName { get; }
        public string? FirstName { get; }
        public string? MiddleName { get; }
        public string? DepartmentId { get; }

        public GetEmployeeDTO(string? id, string? lastName, string? firstName, string? middleName, string? departmentId)
        {
            this.Id = id;
            this.LastName = lastName;
            this.FirstName = firstName;
            this.MiddleName = middleName;
            this.DepartmentId = departmentId;
        }
    }
}
