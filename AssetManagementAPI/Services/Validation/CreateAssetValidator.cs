using AssetManagementAPI.DTO;
using FluentValidation;

namespace AssetManagementAPI.Services.Validation
{
    public class CreateAssetValidator : AbstractValidator<CreateAssetDTO>
    {
        public CreateAssetValidator()
        {
            RuleFor(x => x.Type)
                .Matches(@"^(?!\s*$)[ -~]{2,}$")
                .OverridePropertyName("type")
                .WithMessage("Invalid type.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches(@"^[\p{L}0-9_!¡?÷?¿\/\\+=@#$%ˆ&*(){}|~<>;:[\]\^]{2,}$", System.Text.RegularExpressions.RegexOptions.ECMAScript)
                .OverridePropertyName("name")
                .WithMessage("Invalid asset name.");
        }
    }
}
