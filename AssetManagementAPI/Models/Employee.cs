using AssetManagementAPI.DTO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AssetManagementAPI.Models
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public required string Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public Department? Department { get; set; }
        public string? AccountId { get; set; }

        public GetEmployeeDTO ToDto()
        {
            return new GetEmployeeDTO(
                id: this.Id,
                lastName: this.LastName,
                firstName: this.FirstName,
                middleName: this.MiddleName,
                departmentId: this.Department?.Id
            );
        }
    }
}
