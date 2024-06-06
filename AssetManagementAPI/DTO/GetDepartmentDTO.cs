namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class GetDepartmentDTO
    {
        public string? Id { get; }
        public string? Name { get; }

        public GetDepartmentDTO(string? id, string? name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
