using MultiVendorECommerce.Attributes;
using MultiVendorECommerce.Shared.Enums;

namespace MultiVendorECommerce.Constants
{
    public static class Permissions
    {
        [PermissionType(UserType.Internal)]

        public static class Admin
        {
            public static class Roles
            {
                public const string View   = "Permissions.Admin.Roles.View";
                public const string Create = "Permissions.Admin.Roles.Create";
                public const string Edit   = "Permissions.Admin.Roles.Edit";
                public const string Delete = "Permissions.Admin.Roles.Delete";
            }
            public static class Users
            {
                public const string ViewInternal = "Permissions.Admin.Users.ViewInternal";
                public const string ViewInRole = "Permissions.Admin.Users.ViewInRole";
                public const string AddInternal = "Permissions.Admin.Users.AddInternal";
                public const string UpdateUserRoles = "Permissions.Admin.Users.UpdateUserRoles";
                public const string Suspend = "Permissions.Admin.Users.Suspend";
                public const string Activate = "Permissions.Admin.Users.Activate";
            }

            public static class Home
            {
                public const string View = "Permissions.Admin.Home.View";
                public const string Create = "Permissions.Admin.Home.Create";
                public const string Edit = "Permissions.Admin.Home.Edit";
                public const string Delete = "Permissions.Admin.Home.Delete";

            }
            public static class Category
            {
                public const string View   = "Permissions.Admin.Category.View";
                public const string Create = "Permissions.Admin.Category.Create";
                public const string Edit   = "Permissions.Admin.Category.Edit";
                public const string Delete = "Permissions.Admin.Category.Delete";

            }
            public static class Customer
            {
                public const string ViewAll   = "Permissions.Admin.Customer.ViewAll";
                public const string Activate = "Permissions.Admin.Customer.Activate";
                public const string Block   = "Permissions.Admin.Customer.Block";
                public const string ViewDetails = "Permissions.Admin.Customer.ViewDetails";

            }
            public static class Vendor
            {
                public const string ViewAprroved   = "Permissions.Admin.Vendor.ViewAprroved";
                public const string ViewPending   = "Permissions.Admin.Vendor.ViewPending";
                public const string ViewRejected   = "Permissions.Admin.Vendor.ViewRejected";
                public const string ViewDetails   = "Permissions.Admin.Vendor.ViewDetails";
                public const string Aprrove   = "Permissions.Admin.Vendor.Approve";
                public const string Reject   = "Permissions.Admin.Vendor.Reject";
                public const string Suspend   = "Permissions.Admin.Vendor.Suspend";

            }
            public static class Order
            {
                public const string ViewPending = "Permissions.Admin.Order.ViewPending";
                public const string ViewNewDetails = "Permissions.Admin.Order.ViewNewDetails";
                public const string Confirm = "Permissions.Admin.Order.Confirm";
                public const string Reject = "Permissions.Admin.Order.Reject";

            }

        }

        [PermissionType(UserType.Customer)]
        public static class Customer
        {
            public static class Cart
            {
                
                public const string View   = "Permissions.Customer.Cart.View";
                public const string Create = "Permissions.Customer.Cart.Create";
                public const string Edit   = "Permissions.Customer.Cart.Edit";
                public const string Delete = "Permissions.Customer.Cart.Delete";
            
            }
            public static class Order
            {
                
                public const string View   = "Permissions.Customer.Order.View";
                public const string Create = "Permissions.Customer.Order.Create";
                public const string Edit   = "Permissions.Customer.Order.Edit";
                public const string Delete = "Permissions.Customer.Order.Delete";
            
            }
        }

        [PermissionType(UserType.Vendor)]
        public static class Vendor
        {
            public static class Product
            {

                public const string View   = "Permissions.Vendor.Product.View";
                public const string Create = "Permissions.Vendor.Product.Create";
                public const string Edit   = "Permissions.Vendor.Product.Edit";
                public const string Delete = "Permissions.Vendor.Product.Delete";

            }
        }
    }
}
