namespace MultiVendorECommerce.Shared.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile Image, string folderPass);
        void DeleteImage(string imageName, string folderPass);
    }
}
