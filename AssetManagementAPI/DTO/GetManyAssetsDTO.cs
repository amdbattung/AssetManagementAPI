using System.Text.Json.Serialization;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class GetManyAssetsDTO
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public int ItemCount { get; }
        [JsonPropertyName("data")]
        public IEnumerable<GetAssetDTO> Assets { get; }

        public GetManyAssetsDTO(int pageNumber, int pageSize, int itemCount, IEnumerable<GetAssetDTO> assets)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.ItemCount = itemCount;
            this.Assets = assets;
        }
    }
}
