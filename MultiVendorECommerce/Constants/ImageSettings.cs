namespace MultiVendorECommerce.Constants
{
    public  class ImageSettings
    {
        public const string UsersImagesFolder = "UsersImages";
        public const string ProudctsImagesFolder = "ProductsImages";
        public const string CategoriesImagesFolder = "CategoriesImages";
        public const string RequestPath = "/Uploads";
        public const string AllowedExtensions = ".jpg,.jpeg,.png";
        public const int MaxFileSizeInMB = 2;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;

        public static string UploadsRoot { get; private set; } 

        public static void Configure(string contentRootPath)
        {
            UploadsRoot = Path.Combine(contentRootPath, "Storage", "Uploads");

        }


        public static string GetFolderFullPath(string folderName)
        {
            return Path.Combine(UploadsRoot, folderName);
        }

        public static string GetImageUrl(string folderName, string fileName)
        {
            return $"{RequestPath}/{folderName}/{fileName}";
        }


    }
}
