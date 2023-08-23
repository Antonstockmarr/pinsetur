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
        public string Uri => ContainerUri + "/" + Name;
        public string? Description { get; set; } 

        public ImageDto ToImageDto()
        {
            return new ImageDto { Id = Id, Year = Year, Uri = Uri, Description = Description};
        }


    }
}
