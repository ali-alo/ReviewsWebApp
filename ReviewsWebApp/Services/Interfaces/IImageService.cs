namespace ReviewsWebApp.Services.Interfaces
{
    public interface IImageService
    {
        void UploadImageToAzure(IFormFile file);
    }
}
