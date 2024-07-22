using AssetManagementAPI.Services;
using AssetManagementAPI.Services.Policies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace AssetManagementAPI.Extensions
{
    public static class AuthServiceExtension
    {
        public static IServiceCollection AddKeycloakAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication()
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.MetadataAddress = $"{configuration.GetValue<string>("KeycloakRealm")}/.well-known/openid-configuration";
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = $"preferred_username",
                    ValidAudience = "account",
                    ValidateIssuer = false,
                };
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("email_verified", "true")
                    .Build();

                options.AddPolicy(
                    "UserIsEmployeePolicy",
                    policy => policy.Requirements.Add(new UserIsEmployeeAuthorizationRequirement()));
            });

            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
            services.AddScoped<IAuthorizationHandler, UserIsEmployeeAuthorizationHandler>();

            return services;
        }
    }
}
