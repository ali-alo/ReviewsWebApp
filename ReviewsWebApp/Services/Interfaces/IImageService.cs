namespace ReviewsWebApp.Services.Interfaces
{
    public interface IImageService
    {
        string GetContainerLink();
        Task UploadImageToAzure(IFormFile file);
    }
}
