using Azure.Data.Tables;
using Azure.Storage.Blobs;
using stockmarrdk_api.Models;
using stockmarrdk_api.TableEntities;

namespace stockmarrdk_api.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly TableClient _imageTableClient;
        private readonly string containerUri;


        public ImageRepository(IConfiguration configuration, TableServiceClient tableServiceClient, BlobServiceClient blobServiceClient)
        {
            string? storageTableName = configuration.GetValue<string>("ImagesTableName");
            _imageTableClient = tableServiceClient.GetTableClient(storageTableName);
            string? storageContainerName = configuration.GetValue<string>("ImagesContainerName");

            BlobContainerClient imageContainerClient = blobServiceClient.GetBlobContainerClient(storageContainerName);
            containerUri = imageContainerClient.Uri.ToString();
        }

        public List<Image> GetAllImages()
        {
            return _imageTableClient.Query<ImageEntity>().Select(imageEntity => imageEntity.ToImage(containerUri)).OrderByDescending(image => image.Year).ToList();
        }

        public Image? GetImageById(int id)
        {
            ImageEntity? imageEntity = _imageTableClient.Query<ImageEntity>(imageEntity => imageEntity.PartitionKey == "Image" && imageEntity.RowKey == id.ToString()).SingleOrDefault();
            if (imageEntity is null)
                return null;
            else
            {
                return (Image?)imageEntity.ToImage(containerUri);
            }
        }

        public void UploadImage(Image image)
        {
            _imageTableClient.AddEntity(new ImageEntity(image));
        }

        public Image? DeleteImageById(int id)
        {
            Image? image = GetImageById(id);
            _imageTableClient.DeleteEntity(partitionKey: "Image", rowKey: id.ToString());
            return image;
        }

        public void UpdateImage(Image image)
        {
            _imageTableClient.UpdateEntity(entity: new ImageEntity(image), ifMatch: Azure.ETag.All, mode: TableUpdateMode.Replace);
        }
    }
}