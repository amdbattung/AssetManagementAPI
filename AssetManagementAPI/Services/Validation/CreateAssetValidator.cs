using AssetManagementAPI.DTO;
using FluentValidation;

namespace AssetManagementAPI.Services.Validation
{
    public class CreateAssetValidator : AbstractValidator<CreateAssetDTO>
    {
        public CreateAssetValidator()
        {
            RuleFor(x => x.Type)
                .Matches(@"\[ -~]{2,}/g")
                .WithMessage("Invalid type.");

            RuleFor(x => x.Name)
                .Matches(@"/^[\p{L}0-9_!¡?÷?¿\/\\+=@#$%ˆ&*(){}|~<>;:[\]\^]{2,}$/u")
                .WithMessage("Invalid asset name.");
        }
    }
}
