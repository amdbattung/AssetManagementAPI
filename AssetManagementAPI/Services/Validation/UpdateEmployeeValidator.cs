using AssetManagementAPI.DTO;
using FluentValidation;

namespace AssetManagementAPI.Services.Validation
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDTO>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(x => x.LastName)
                .Matches(@"^(?!\s*$).*")
                .WithMessage("Invalid last name.");

            RuleFor(x => x.FirstName)
                .Matches(@"^(?!\s*$).*")
                .WithMessage("Invalid first name.");

            RuleFor(x => x.MiddleName)
                .Matches(@"^(?!\s*$).*")
                .WithMessage("Invalid middle name.");
        }
    }
}
