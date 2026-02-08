namespace PermissionBasedAuz.ViewModels
{
    public class RolePermissionsVM
    {
        public string RoleId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public List<CheckBox> Permissions { get; set; } = new List<CheckBox>();
    }
}
