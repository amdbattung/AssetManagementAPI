using AssetManagementAPI.Services.Extensions;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins((builder.Configuration.GetValue<string>("AllowedOrigins", "") ?? "").Split(";"))
            .WithHeaders(HeaderNames.Origin, HeaderNames.XRequestedWith, HeaderNames.ContentType, HeaderNames.Accept);
    });
});

builder.Services.AddKeycloakAuth(builder.Configuration);

builder.Services.AddControllers(options => {
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRepository();
builder.Services.AddValidator();

builder.Services.AddEFCore(builder.Configuration);

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
