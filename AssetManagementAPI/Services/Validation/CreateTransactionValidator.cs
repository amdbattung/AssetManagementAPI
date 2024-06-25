using AssetManagementAPI.DTO;
using FluentValidation;

namespace AssetManagementAPI.Services.Validation
{
    public class CreateTransactionValidator : AbstractValidator<CreateTransactionDTO>
    {
        public CreateTransactionValidator()
        {
            RuleFor(x => x.AssetId)
                .NotEmpty()
                .WithMessage("Asset required.");

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Transaction type required.");

            RuleFor(x => x.TransactorId)
                .NotEmpty()
                .WithMessage("Transactor required.");

            RuleFor(x => x.TransacteeId)
                .NotEmpty()
                .WithMessage("Transactee required.");

            RuleFor(x => x.Reason)
                .Matches(@"^(?!\s*$)[ -~]{2,}$")
                .WithMessage("Invalid reason.");

            RuleFor(x => x.Remark)
                .Matches(@"^(?!\s*$)[ -~]{2,}$")
                .WithMessage("Invalid reason.");
        }
    }
}
