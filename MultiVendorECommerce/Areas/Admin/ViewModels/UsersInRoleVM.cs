namespace MultiVendorECommerce.Areas.Admin.ViewModels
{
    public class UsersInRoleVM
    {
        public string RoleId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public List<InternalUserVM> Users { get; set; } = new List<InternalUserVM>();
    }
}
