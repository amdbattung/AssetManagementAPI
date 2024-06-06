using AssetManagementAPI.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagementAPI.Models
{
    public class Department
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public required string Id { get; set; }
        public required string Name { get; set; }

        public GetDepartmentDTO ToDto()
        {
            return new GetDepartmentDTO(
                id: this.Id,
                name: this.Name
            );
        }
    }
}
