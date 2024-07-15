using AssetManagementAPI.DTO;
using FluentValidation;

namespace AssetManagementAPI.Services.Validation
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDTO>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.LastName)
                .Matches(@"^(?!\s*$).*")
                .OverridePropertyName("lastName")
                .WithMessage("Invalid last name.");

            RuleFor(x => x.FirstName)
                .Matches(@"^(?!\s*$).*")
                .OverridePropertyName("firstName")
                .WithMessage("Invalid first name.");

            RuleFor(x => x.MiddleName)
                .Matches(@"^(?!\s*$).*")
                .OverridePropertyName("middleName")
                .WithMessage("Invalid middle name.");
        }
    }
}
