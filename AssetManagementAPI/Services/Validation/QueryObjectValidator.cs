using AssetManagementAPI.Services.Helpers;
using FluentValidation;

namespace AssetManagementAPI.Services.Validation
{
    public class QueryObjectValidator : AbstractValidator<QueryObject>
    {
        public QueryObjectValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Invalid page number.")
                .WithErrorCode("QERR0001");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Invalid page size.")
                .LessThanOrEqualTo(50)
                .WithMessage("Page size too large.")
                .WithErrorCode("QERR0002");
        }
    }
}
