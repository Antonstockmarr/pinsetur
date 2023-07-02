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

        public List<Image> GetAllImages()
        {
            return _imageTableClient.Query<ImageEntity>().Select(imageEntity => imageEntity.ToImage()).OrderByDescending(image => image.Year).ToList();
        }

        public Image? GetImageById(int id)
        {
            ImageEntity? imageEntity = _imageTableClient.Query<ImageEntity>(imageEntity => imageEntity.PartitionKey == "Image" && imageEntity.RowKey == id.ToString()).SingleOrDefault();
            if (imageEntity is null)
                return null;
            else
            {
                return (Image?)imageEntity.ToImage();
            }
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

        public void UploadImage(Image image)
        {
            _imageTableClient.AddEntity(new ImageEntity(image));
        }
    }
}