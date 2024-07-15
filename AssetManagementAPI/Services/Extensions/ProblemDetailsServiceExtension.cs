using System.Diagnostics;

namespace AssetManagementAPI.Services.Extensions
{
    public static class ProblemDetailsServiceExtension
    {
        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            options.CustomizeProblemDetails = (context) =>
            {
                context.ProblemDetails.Extensions.Remove("traceId");
            });

            return services;
        }
    }
}
