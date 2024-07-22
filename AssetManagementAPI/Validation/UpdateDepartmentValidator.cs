using AssetManagementAPI.DTO;
using FluentValidation;

namespace AssetManagementAPI.Validation
{
    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentDTO>
    {
        public UpdateDepartmentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .OverridePropertyName("name")
                .WithMessage("Name is required.");
        }
    }
}
