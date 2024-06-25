using System.Text.Json;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class UpdateAssetDTO
    {
        public string? Type { get; set; }
        public string? Name { get; set; }
        public JsonElement? Info { get; set; }
        public string? ProprietorId { get; set; }
        public string? CustodianId { get; set; }
        public bool? IsActive { get; set; }
    }
}
