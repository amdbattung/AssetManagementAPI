using Microsoft.AspNetCore.Mvc;

namespace AssetManagementAPI.Models
{
    public class QueryObject
    {
        [FromQuery(Name = "q")]
        public string? Query { get; set; }
        [FromQuery(Name = "page")]
        public int? PageNumber { get; set; }
        [FromQuery(Name = "page-size")]
        public int? PageSize { get; set; }
    }
}
