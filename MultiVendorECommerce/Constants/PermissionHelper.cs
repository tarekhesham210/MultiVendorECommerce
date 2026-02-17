using MultiVendorECommerce.Attributes;
using MultiVendorECommerce.Shared.Enums;
using System.Reflection;

namespace MultiVendorECommerce.Constants
{
    public static class PermissionHelper
    {
        public static List<string?> GetAllPermissions(UserType userType)
        {
            var Permissions = new List<string?>();

            var nestedTypes = typeof(Permissions).GetNestedTypes()
                    .Where(t => t.GetCustomAttribute<PermissionTypeAttribute>()?.AllowedUserType == userType);
            foreach (var type in nestedTypes)
            {
                Permissions.AddRange(GetPermissionsFromType(type));
            }

            return Permissions;
        }

        private static List<string?> GetPermissionsFromType(Type type)
        {
            var permissions = new List<string?>();

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                             .Where(f => f.FieldType == typeof(string))
                             .Select(f => f.GetValue(null)?.ToString());

            permissions.AddRange(fields);

            var nestedTypes = type.GetNestedTypes();
            foreach (var nested in nestedTypes)
            {
                permissions.AddRange(GetPermissionsFromType(nested));
            }

            return permissions;
        }










        //public static List<string?> GetAllPermissions()
        //{
        //    return typeof(Permissions).GetNestedTypes()
        //        .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static))
        //        .Where(f => f.FieldType == typeof(string))
        //        .Select(f => f.GetValue(null)?.ToString()).ToList();

        //}
    }
}
