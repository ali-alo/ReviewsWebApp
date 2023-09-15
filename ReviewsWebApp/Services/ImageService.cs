using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using ReviewsWebApp.Models;
using ReviewsWebApp.Options;
using ReviewsWebApp.Services.Interfaces;

namespace ReviewsWebApp.Services
{
    public class ImageService : IImageService
    {
        private readonly AzureOptions _azureOptions;

        public ImageService(IOptions<AzureOptions> azureOptions)
        {
            _azureOptions = azureOptions.Value;
        }

        private bool IsImageExtension(string fileExtension)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return allowedExtensions.Contains(fileExtension);
        }

        private async Task<MemoryStream> ConvertFormFileToMemoryStream(IFormFile file)
        {
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

        private string GenerateUniqueBlobName(string fileExtension) =>
            Guid.NewGuid().ToString() + fileExtension;

        private BlobClient GetBlobClient(string blobName)
        {
            var blobContainerClient = new BlobContainerClient(_azureOptions.ConnectionString,
                                                              _azureOptions.Container);
            return blobContainerClient.GetBlobClient(blobName);
        }

        private async Task UploadFileToAzureBlobStorage(string blobName, MemoryStream stream)
        {
            var blobClient = GetBlobClient(blobName);
            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = "image/bitmap" }
            });
        }

        public string GetContainerLink() => _azureOptions.ContainerLink;

        public async Task<string> UploadImageToAzure(IFormFile file)
        {
            if (file == null)
                return string.Empty;
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!IsImageExtension(fileExtension))
                return string.Empty;
            using var fileUploadStream = await ConvertFormFileToMemoryStream(file);
            string uniqueName = GenerateUniqueBlobName(fileExtension);
            await UploadFileToAzureBlobStorage(uniqueName, fileUploadStream);
            return uniqueName;
        }

        public async Task<List<Image>> UploadImagesToAzure(List<IFormFile> files)
        {
            var images = new List<Image>();
            foreach (var imgFile in files)
            {
                string imageGuid = await UploadImageToAzure(imgFile);
                if (!string.IsNullOrEmpty(imageGuid))
                    images.Add(new Image { ImageGuid = imageGuid});
            }
            return images;
        }

        public async Task DeleteImageFromAzure(string blobName)
        {
            var blobClient = GetBlobClient(blobName);
            if (await blobClient.ExistsAsync())
                await blobClient.DeleteAsync();
        }

        public async Task DeleteImagesFromAzure(IEnumerable<string> blobNames)
        {
            foreach (var blobName in blobNames)
                await DeleteImageFromAzure(blobName);
        }
    }
}