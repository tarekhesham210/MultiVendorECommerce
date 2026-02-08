using PermissionBasedAuz.Shared.Enums;

namespace PermissionBasedAuz.Areas.Admin.ViewModels
{
    public class InternalUserVM
    {
        public string Id { get; set; }=null!;
        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public Userstatus Userstatus { get; set; }


    }
}
