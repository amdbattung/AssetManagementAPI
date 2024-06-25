using AssetManagementAPI.DTO;
using FluentValidation;

namespace AssetManagementAPI.Services.Validation
{
    public class UpdateAssetValidator : AbstractValidator<UpdateAssetDTO>
    {
        public UpdateAssetValidator()
        {
            RuleFor(x => x.Type)
                .Matches(@"^(?!\s*$)[ -~]{2,}$")
                .WithMessage("Invalid type.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches(@"^[\p{L}0-9_!¡?÷?¿\/\\+=@#$%ˆ&*(){}|~<>;:[\]\^]{2,}$", System.Text.RegularExpressions.RegexOptions.ECMAScript)
                .WithMessage("Invalid asset name.");
        }
    }
}
