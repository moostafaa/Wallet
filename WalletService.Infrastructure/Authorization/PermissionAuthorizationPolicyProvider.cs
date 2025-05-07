using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using WalletService.Domain.Authorization;

namespace WalletService.Infrastructure.Authorization
{
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options) : base(options)
        {
        }
        
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            
            if (policy != null)
                return policy;
                
            if (Enum.TryParse<Permission>(policyName, out var permission))
            {
                var policyBuilder = new AuthorizationPolicyBuilder();
                policyBuilder.AddRequirements(new PermissionRequirement(permission));
                return policyBuilder.Build();
            }
            
            return null;
        }
    }
}