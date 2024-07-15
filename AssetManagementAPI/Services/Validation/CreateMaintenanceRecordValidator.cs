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
                .OverridePropertyName("assetId")
                .WithMessage("Asset required.");

            RuleFor(x => x.Action)
                .NotEmpty()
                .OverridePropertyName("action")
                .WithMessage("Maintenance action required.");

            RuleFor(x => x.Reason)
                .Matches(@"^(?!\s*$)[ -~]{2,}$")
                .OverridePropertyName("reason")
                .WithMessage("Invalid reason.");

            RuleFor(x => x.Comment)
                .Matches(@"^(?!\s*$)[ -~]{2,}$")
                .OverridePropertyName("comment")
                .WithMessage("Invalid comment.");
        }
    }
}
