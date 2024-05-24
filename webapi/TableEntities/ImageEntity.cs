using Azure;
using Azure.Data.Tables;
using stockmarrdk_api.Models;

namespace stockmarrdk_api.TableEntities
{
    public class ImageEntity : ITableEntity
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Extension { get; set; } = "";
        public string? Description { get; set; }
        public string? UploadedBy { get; set; }

        // ITableEntity Properties
        public virtual string PartitionKey { get; set; } = "Image";
        public virtual string RowKey { get => Id.ToString(); set => Id = int.Parse(value); }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public ImageEntity() { }

        public ImageEntity(Image image)
        {
            Id = image.Id;
            Year = image.Year;
            Extension = image.Extension;
            RowKey = Id.ToString();
            Description = image.Description;
            UploadedBy = image.UploadedBy;
        }

        public Image ToImage(string containerUri)
        {   
            return new Image { Id = Id, Year = Year, Extension = Extension, ContainerUri = containerUri, Description = Description, UploadedBy = UploadedBy };
        }
    }
}
