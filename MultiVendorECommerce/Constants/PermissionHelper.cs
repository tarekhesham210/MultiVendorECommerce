using System.Reflection;

namespace PermissionBasedAuz.Constants
{
    public static class PermissionHelper
    {
        public static List<string?> GetAllPermissions()
        {
            return typeof(Permissions).GetNestedTypes()
                .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static))
                .Where(f => f.FieldType == typeof(string))
                .Select(f => f.GetValue(null)?.ToString()).ToList();
            //{
                //// Roles Permissions
                //Permissions.Roles.View,
                //Permissions.Roles.Create,
                //Permissions.Roles.Edit,
                //Permissions.Roles.Delete,
                //// Users Permissions
                //Permissions.Users.View,
                //Permissions.Users.Create,
                //Permissions.Users.Edit,
                //Permissions.Users.Delete,
            //};
        }
    }
}
