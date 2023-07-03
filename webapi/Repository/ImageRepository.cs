using Azure.Data.Tables;
using stockmarrdk_api.Models;
using stockmarrdk_api.TableEntities;

namespace stockmarrdk_api.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly TableClient _imageTableClient;


        public ImageRepository(IConfiguration configuration, TableServiceClient tableServiceClient)
        {
            string? storageTableName = configuration.GetValue<string>("ImagesTableName");
            _imageTableClient = tableServiceClient.GetTableClient(storageTableName);
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
    }
}