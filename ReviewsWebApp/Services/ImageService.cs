using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using ReviewsWebApp.Options;
using ReviewsWebApp.Services.Interfaces;

namespace ReviewsWebApp.Services
{
    // TODO: Add Delete from blob storage functionality 
    public class ImageService : IImageService
    {
        private readonly AzureOptions _azureOptions;

        public ImageService(IOptions<AzureOptions> azureOptions)
        {
            _azureOptions = azureOptions.Value;
        }

        public string GetContainerLink() => _azureOptions.ContainerLink;

        public async Task<string> UploadImageToAzure(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!IsImageExtension(fileExtension))
                return string.Empty;
            using var fileUploadStream = await ConvertFormFileToMemoryStream(file);
            string uniqueName = GenerateUniqueBlobName(fileExtension);
            await UploadFileToAzureBlobStorage(uniqueName, fileUploadStream);
            return uniqueName;
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

        private async Task UploadFileToAzureBlobStorage(string blobName, MemoryStream stream)
        {
            var blobContainerClient = new BlobContainerClient(_azureOptions.ConnectionString,
                                                              _azureOptions.Container);
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = "image/bitmap" }
            });
        }
    }
}