using Microsoft.OpenApi.Models;

namespace AssetManagementAPI.Services.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Asset Management API", Version = "v1" });
                c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri($"{configuration.GetValue<string>("KeycloakRealm")}/protocol/openid-connect/token")
                        }
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "OAuth2"
                            },
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }
    }
}
