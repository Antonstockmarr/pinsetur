using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using stockmarrdk_api.Models;

namespace stockmarrdk_api.Repository
{
    public class ImageDataRepository : IImageDataRepository
    {
        private readonly BlobContainerClient _imageContainerClient;
        
        public ImageDataRepository(IConfiguration configuration, BlobServiceClient blobServiceClient)
        {
            string? storageContainerName = configuration.GetValue<string>("ImagesContainerName");

            _imageContainerClient = blobServiceClient.GetBlobContainerClient(storageContainerName);
        }

        public async Task DeleteImageDataByImage(Image image)
        {
            BlobClient imageBlob = _imageContainerClient.GetBlobClient(image.Name);

            if (!imageBlob.Exists())
            {
                throw new Exception($"Blob {image.Name} does not exists");
            }

            await imageBlob.DeleteAsync();
        }

        public async Task<ImageData?> GetImageDataFromImage(Image image)
        {
            BlobClient imageBlob = _imageContainerClient.GetBlobClient(image.Name);

            if (!imageBlob.Exists())
            {
                return null;
            }

            BlobDownloadResult blob = await imageBlob.DownloadContentAsync();

            return new ImageData
            {
                Id = image.Id,
                ContentType = blob.Details.ContentType,
                Content = blob.Content.ToArray()
            };
        }

        public async Task UploadImageData(ImageData imageData, Image image)
        {
            BlobClient imageBlob = _imageContainerClient.GetBlobClient(image.Name);

            if (imageBlob.Exists())
            {
                throw new Exception("Blob already exists");
            }

            await imageBlob.UploadAsync(BinaryData.FromBytes(imageData.Content));
        }
    }
}
