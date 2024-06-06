using AssetManagementAPI.Interfaces;
using AssetManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace AssetManagementAPI.Services.Policies
{
    public class UserIsEmployeeAuthorizationHandler : AuthorizationHandler<UserIsEmployeeAuthorizationRequirement>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public UserIsEmployeeAuthorizationHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIsEmployeeAuthorizationRequirement requirement)
        {
            var httpContext = (HttpContext?)context.Resource;
            var claimsPrincipalSubject = context.User?.FindFirst("sub")?.Value;

            if (httpContext == null || string.IsNullOrWhiteSpace(claimsPrincipalSubject))
            {
                return;
            }
            else
            {
                string id = httpContext?.GetRouteValue("id")?.ToString() ?? "";

                Employee? employee = await _employeeRepository.GetByIdAsync(id);

                if (employee?.AccountId == claimsPrincipalSubject)
                {
                    context.Succeed(requirement);
                }
            }
            return;
        }
    }
}
