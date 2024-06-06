using AssetManagementAPI.Data;
using AssetManagementAPI.Models;
using AssetManagementAPI.Services.Extensions;
using AssetManagementAPI.Services.Helpers;
using AssetManagementAPI.Services.Policies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Asset Management API", Version = "v1" });
    c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("http://10.88.244.105:8080/realms/asset-management/protocol/openid-connect/token")
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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins((builder.Configuration.GetValue<string>("AllowedOrigins", "*") ?? "*").Split(";"))
                .WithHeaders(HeaderNames.Origin, HeaderNames.XRequestedWith, HeaderNames.ContentType, HeaderNames.Accept);
        });
});

builder.Services.AddAuthentication()
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.MetadataAddress = $"http://10.88.244.105:8080/realms/asset-management/.well-known/openid-configuration";
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = $"preferred_username",
                    ValidAudience = "account",
                    ValidateIssuer = false,
                };
            });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("email_verified", "true")
        .Build();

    options.AddPolicy(
        "UserIsEmployeePolicy",
        policy => policy.Requirements.Add(new UserIsEmployeeAuthorizationRequirement()));
});

builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
builder.Services.AddScoped<IAuthorizationHandler, UserIsEmployeeAuthorizationHandler>();
/*builder.Services.AddSingleton<IAuthorizationHandler, UserIsEmployeeAuthorizationHandler>();*/

builder.Services.AddControllers(options => {
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRepository();
builder.Services.AddValidator();

// EF Core
var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("AssetManagementContext"));
dataSourceBuilder.MapEnum<MaintenanceRecord.MaintenanceAction>();
dataSourceBuilder.MapEnum<Transaction.TransactionType>();
var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<DataContext>(options =>
{
    options
        .UseNpgsql(dataSource);
});

// Build app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
