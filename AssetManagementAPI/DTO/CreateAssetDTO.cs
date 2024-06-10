using System.Text.Json;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class CreateAssetDTO
    {
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Info { get; set; }
        public string? ProprietorId { get; set; }
        public string? CustodianId { get; set; }
    }
}
