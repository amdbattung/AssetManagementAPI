using AssetManagementAPI.DTO;
using FluentValidation;

namespace AssetManagementAPI.Validation
{
    public class CreateTransactionValidator : AbstractValidator<CreateTransactionDTO>
    {
        public CreateTransactionValidator()
        {
            RuleFor(x => x.AssetId)
                .NotEmpty()
                .OverridePropertyName("assetId")
                .WithMessage("Asset required.");

            RuleFor(x => x.Type)
                .NotEmpty()
                .OverridePropertyName("type")
                .WithMessage("Transaction type required.");

            RuleFor(x => x.TransactorId)
                .NotEmpty()
                .OverridePropertyName("transactorId")
                .WithMessage("Transactor required.");

            RuleFor(x => x.TransacteeId)
                .NotEmpty()
                .OverridePropertyName("transacteeId")
                .WithMessage("Transactee required.");

            RuleFor(x => x.Reason)
                .Matches(@"^(?!\s*$)[ -~]{2,}$")
                .OverridePropertyName("reason")
                .WithMessage("Invalid reason.");

            RuleFor(x => x.Remark)
                .Matches(@"^(?!\s*$)[ -~]{2,}$")
                .OverridePropertyName("remark")
                .WithMessage("Invalid reason.");
        }
    }
}
