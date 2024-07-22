using AssetManagementAPI.Data;
using AssetManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AssetManagementAPI.Extensions
{
    public static class EFCoreServiceExtension
    {
        public static IServiceCollection AddEFCore(this IServiceCollection services, IConfiguration configuration)
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("AssetManagementContext"));
            dataSourceBuilder.UseNodaTime();
            dataSourceBuilder.MapEnum<MaintenanceRecord.MaintenanceAction>();
            dataSourceBuilder.MapEnum<Transaction.TransactionType>();
            var dataSource = dataSourceBuilder.Build();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(dataSource, o => o.UseNodaTime());
            });

            return services;
        }
    }
}
