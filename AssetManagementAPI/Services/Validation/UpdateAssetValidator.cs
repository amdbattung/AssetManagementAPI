using AssetManagementAPI.DTO;
using FluentValidation;
using System.Text.Json;

namespace AssetManagementAPI.Services.Validation
{
    public class UpdateAssetValidator : AbstractValidator<UpdateAssetDTO>
    {
        public UpdateAssetValidator()
        {
            RuleFor(x => x.Type)
                .Matches(@"\[ -~]{2,}/g")
                .WithMessage("Invalid type.");

            RuleFor(x => x.Name)
                .Matches(@"/^[\p{L}0-9_!¡?÷?¿\/\\+=@#$%ˆ&*(){}|~<>;:[\]\^]{2,}$/u")
                .WithMessage("Invalid asset name.");

            RuleFor(x => x.Info)
                .Must(IsValidJson)
                .WithMessage("Invalid Info, must be a JSON.");
        }

        private bool IsValidJson(string? jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return false;
            }

            try
            {
                JsonDocument.Parse(jsonString);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}
