using AssetManagementAPI.DTO;
using FluentValidation;

namespace AssetManagementAPI.Services.Validation
{
    public class UpdateMaintenanceRecordValidator : AbstractValidator<UpdateMaintenanceRecordDTO>
    {
        public UpdateMaintenanceRecordValidator()
        {
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
