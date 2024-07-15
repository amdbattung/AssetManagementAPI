using AssetManagementAPI.DTO;
using AssetManagementAPI.Services.Helpers;
using AssetManagementAPI.Services.Validation;
using FluentValidation;

namespace AssetManagementAPI.Services.Extensions
{
    public static class ValidatorServiceExtension
    {
        public static IServiceCollection AddValidator(this IServiceCollection services)
        {
            // Register validators here
            services.AddScoped<IValidator<CreateAssetDTO>, CreateAssetValidator>();
            services.AddScoped<IValidator<UpdateAssetDTO>, UpdateAssetValidator>();
            services.AddScoped<IValidator<CreateDepartmentDTO>, CreateDepartmentValidator>();
            services.AddScoped<IValidator<UpdateDepartmentDTO>, UpdateDepartmentValidator>();
            services.AddScoped<IValidator<CreateEmployeeDTO>, CreateEmployeeValidator>();
            services.AddScoped<IValidator<UpdateEmployeeDTO>, UpdateEmployeeValidator>();
            services.AddScoped<IValidator<CreateTransactionDTO>, CreateTransactionValidator>();
            services.AddScoped<IValidator<UpdateTransactionDTO>, UpdateTransactionValidator>();
            services.AddScoped<IValidator<CreateMaintenanceRecordDTO>, CreateMaintenanceRecordValidator>();
            services.AddScoped<IValidator<UpdateMaintenanceRecordDTO>, UpdateMaintenanceRecordValidator>();
            services.AddScoped<IValidator<QueryObject>, QueryObjectValidator>();

            return services;
        }
    }
}
