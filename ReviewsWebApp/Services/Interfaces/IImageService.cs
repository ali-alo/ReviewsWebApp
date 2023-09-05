namespace ReviewsWebApp.Services.Interfaces
{
    public interface IImageService
    {
        string GetContainerLink();
        Task<string> UploadImageToAzure(IFormFile file);
    }
}
