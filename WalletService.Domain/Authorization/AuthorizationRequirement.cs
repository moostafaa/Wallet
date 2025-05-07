using Microsoft.AspNetCore.Authorization;
using WalletService.Domain.Authorization;

namespace WalletService.Domain.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public Permission RequiredPermission { get; }
        
        public PermissionRequirement(Permission permission)
        {
            RequiredPermission = permission;
        }
    }
}