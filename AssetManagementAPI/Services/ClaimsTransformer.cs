using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;

namespace AssetManagementAPI.Services
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var result = principal.Clone();
            if (result.Identity is not ClaimsIdentity identity)
            {
                return Task.FromResult(result);
            }

            var realmAccessValue = principal.FindFirst("realm_access")?.Value;
            if (string.IsNullOrWhiteSpace(realmAccessValue))
            {
                return Task.FromResult(result);
            }

            using var realmAccess = JsonDocument.Parse(realmAccessValue);
            var userRoles = realmAccess
                .RootElement
                .GetProperty("roles");

            foreach (var role in userRoles.EnumerateArray())
            {
                var value = role.GetString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, value));
                }
            }

            return Task.FromResult(result);
        }
    }
}
