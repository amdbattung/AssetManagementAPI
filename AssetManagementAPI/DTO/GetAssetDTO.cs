using System.Text.Json;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class GetAssetDTO
    {
        public string? Id { get; }
        public string? Type { get;  }
        public string? Name { get; }
        public JsonElement? Info { get; }
        public string? ProprietorId { get; }
        public string? CustodianId { get; }
        public bool IsActive { get; }

        public GetAssetDTO(string? id, string? type, string? name, JsonElement? info, string? proprietorId, string? custodianId, bool isActive)
        {
            this.Id = id;
            this.Type = type;
            this.Name = name;
            this.Info = info;
            this.ProprietorId = proprietorId;
            this.CustodianId = custodianId;
            this.IsActive = isActive;
        }
    }
}
