using ReviewsWebApp.Models;

namespace ReviewsWebApp.Services.Interfaces
{
    public interface IImageService
    {
        string GetContainerLink();
        Task<string> UploadImageToAzure(IFormFile file);
        Task<List<Image>> UploadImagesToAzure(List<IFormFile> files);
        Task DeleteImageFromAzure(string blobName);
        Task DeleteImagesFromAzure(IEnumerable<string> blobNames);
    }
}
