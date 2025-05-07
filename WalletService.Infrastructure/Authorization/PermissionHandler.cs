using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WalletService.Domain.Authorization;

namespace WalletService.Infrastructure.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userPermissions = context.User.FindFirst("permissions")?.Value;
            if (string.IsNullOrEmpty(userPermissions))
                return Task.CompletedTask;
                
            var permissions = (Permission)long.Parse(userPermissions);
            
            if ((permissions & requirement.RequiredPermission) == requirement.RequiredPermission)
            {
                context.Succeed(requirement);
            }
            
            return Task.CompletedTask;
        }
    }
}