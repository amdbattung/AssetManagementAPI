using AssetManagementAPI.Extensions;
using Microsoft.OpenApi.Models;
using NodaTime;

namespace AssetManagementAPI.Extensions
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

                c.MapType<Instant>(() => new OpenApiSchema { Type = "string", Format = "date-time" });
                c.MapType<LocalDate>(() => new OpenApiSchema { Type = "string", Format = "date" });
                c.MapType<LocalDateTime>(() => new OpenApiSchema { Type = "string", Format = "date-time" });
                c.MapType<OffsetDateTime>(() => new OpenApiSchema { Type = "string", Format = "date-time" });
                c.MapType<ZonedDateTime>(() => new OpenApiSchema { Type = "string", Format = "date-time" });
                c.MapType<Duration>(() => new OpenApiSchema { Type = "string", Format = "duration" });
            });

            return services;
        }
    }
}
