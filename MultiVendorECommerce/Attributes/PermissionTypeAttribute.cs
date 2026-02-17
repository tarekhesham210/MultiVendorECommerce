using MultiVendorECommerce.Shared.Enums;

namespace MultiVendorECommerce.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PermissionTypeAttribute : Attribute
    {
        public UserType AllowedUserType { get; }
        public PermissionTypeAttribute(UserType userType) => AllowedUserType = userType;
    }
}
