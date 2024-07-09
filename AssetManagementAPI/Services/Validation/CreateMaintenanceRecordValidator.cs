using AssetManagementAPI.DTO;
using FluentValidation;

namespace AssetManagementAPI.Services.Validation
{
    public class CreateMaintenanceRecordValidator : AbstractValidator<CreateMaintenanceRecordDTO>
    {
        public CreateMaintenanceRecordValidator()
        {
            RuleFor(x => x.AssetId)
                .NotEmpty()
                .WithMessage("Asset required.");

            RuleFor(x => x.Action)
                .NotEmpty()
                .WithMessage("Maintenance action required.");

            RuleFor(x => x.Reason)
                .Matches(@"^(?!\s*$)[ -~]{2,}$")
                .WithMessage("Invalid reason.");

            RuleFor(x => x.Comment)
                .Matches(@"^(?!\s*$)[ -~]{2,}$")
                .WithMessage("Invalid comment.");
        }
    }
}
