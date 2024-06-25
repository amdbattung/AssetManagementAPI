using AssetManagementAPI.Interfaces;
using AssetManagementAPI.Repositories;

namespace AssetManagementAPI.Services.Extensions
{
    public static class RepositoryServiceExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            //
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
