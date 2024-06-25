using System.Text.Json.Serialization;

namespace AssetManagementAPI.DTO
{
    [Serializable]
    public class GetManyTransactionsDTO
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public int ItemCount { get; }
        [JsonPropertyName("data")]
        public IEnumerable<GetTransactionDTO> Transactions { get; }

        public GetManyTransactionsDTO(int pageNumber, int pageSize, int itemCount, IEnumerable<GetTransactionDTO> transactions)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.ItemCount = itemCount;
            this.Transactions = transactions;
        }
    }
}
