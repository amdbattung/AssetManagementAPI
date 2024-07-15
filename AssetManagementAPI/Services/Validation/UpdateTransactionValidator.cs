using AssetManagementAPI.DTO;
using FluentValidation;

namespace AssetManagementAPI.Services.Validation
{
    public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionDTO>
    {
        public UpdateTransactionValidator()
        {
            RuleFor(x => x.Reason)
                .Matches(@"^(?!\s*$)[ -~]{2,}$")
                .OverridePropertyName("reason")
                .WithMessage("Invalid reason.");

            RuleFor(x => x.Remark)
                .Matches(@"^(?!\s*$)[ -~]{2,}$")
                .OverridePropertyName("comment")
                .WithMessage("Invalid remark.");
        }
    }
}
