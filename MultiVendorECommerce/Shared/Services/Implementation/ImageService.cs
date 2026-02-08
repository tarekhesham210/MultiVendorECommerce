using PermissionBasedAuz.Constants;
using PermissionBasedAuz.Exceptions;
using PermissionBasedAuz.Shared.Services.Interfaces;

namespace PermissionBasedAuz.Shared.Services.Implementation
{
    public class ImageService : IImageService
    {
        public async Task<string> SaveImageAsync(IFormFile Image, string folderName)
        {
            if (Image == null || Image.Length == 0)
                throw new ValidationException("Invalid image");


            var folderPath=Path.Combine(ImageSettings.GetFolderFullPath(folderName));
            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(Image.FileName)}";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var path = Path.Combine(folderPath, imageName);

            await using var stream = new FileStream(
                           path,
                           FileMode.Create,
                           FileAccess.Write,
                           FileShare.None,
                           4096,
                           true); 
            await Image.CopyToAsync(stream);

            return imageName;
        }

        public void DeleteImage(string imageName, string folderName)
        {
            if (!string.IsNullOrWhiteSpace(imageName))
            {
                var folderPath = Path.Combine(ImageSettings.GetFolderFullPath(folderName));
                var path = Path.Combine(folderPath, imageName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        public string GetImageUrl(string folderName, string fileName)
        {
            return ImageSettings.GetImageUrl(folderName, fileName);
        }
    }
}
