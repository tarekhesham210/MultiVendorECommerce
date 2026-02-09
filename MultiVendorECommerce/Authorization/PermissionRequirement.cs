using Microsoft.AspNetCore.Authorization;

namespace MultiVendorECommerce.Authorization
{
    public class PermissionRequirement:IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
