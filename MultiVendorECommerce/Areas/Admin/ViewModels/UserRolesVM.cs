using PermissionBasedAuz.Shared.ViewModels;

namespace PermissionBasedAuz.Areas.Admin.ViewModels
{
    public class UserRolesVM
    {
        public string Id { get; set; }=null!;
        public string? Email { get; set; }
        public List<CheckBox> Roles { get; set; } = new List<CheckBox>();
    }
}
