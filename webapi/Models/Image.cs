using stockmarrdk_api.Dto;

namespace stockmarrdk_api.Models
{
    public class Image
    {
        public required int Id { get; set; }
        public required int Year { get; set; }
        public string ContainerUri { get; set; } = "";
        public string Extension { get; set; } = "";
        public string Name => Year + "/" + Id + Extension;
        public string ThumbName => $"{Year}/{Id}_thumb.jpeg";
        public string Uri => ContainerUri + "/" + Name;
        public string ThumbUri => ContainerUri + "/" + ThumbName;
        public string? Description { get; set; } 
        public string? UploadedBy { get; set; }

        public ImageDto ToImageDto()
        {
            return new ImageDto { Id = Id, Year = Year, Uri = Uri, ThumbUri = ThumbUri, Description = Description, UploadedBy = UploadedBy };
        }
    }
}
