using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Data.Tables;
using stockmarrdk_api.Models;
using stockmarrdk_api.TableEntities;

namespace stockmarrdk_api.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly BlobContainerClient _imageContainerClient;
        private readonly TableClient _imageTableClient;


        public ImageRepository(IConfiguration configuration, BlobServiceClient blobServiceClient, TableServiceClient tableServiceClient)
        {
            string? storageContainerName = configuration.GetValue<string>("ImagesContainerName");
            string? storageTableName = configuration.GetValue<string>("ImagesTableName");

            _imageContainerClient = blobServiceClient.GetBlobContainerClient(storageContainerName);
            _imageTableClient = tableServiceClient.GetTableClient(storageTableName);
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

        Task<List<Image>> IImageRepository.GetAllImages()
        {
            return Task.FromResult(_imageTableClient.Query<ImageEntity>().Select(imageEntity => imageEntity.ToImage()).OrderByDescending(image => image.Year).ToList());
        }

        Task<Image?> IImageRepository.GetImageById(int id)
        {
            ImageEntity? imageEntity = _imageTableClient.Query<ImageEntity>(imageEntity => imageEntity.PartitionKey == "Image" && imageEntity.RowKey == id.ToString()).SingleOrDefault();
            if (imageEntity is null)
                return Task.FromResult<Image?>(null);
            else
            {
                return Task.FromResult((Image?)imageEntity.ToImage());
            }
        }
    }
}