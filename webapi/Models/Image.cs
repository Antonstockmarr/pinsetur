using stockmarrdk_api.Dto;

namespace stockmarrdk_api.Models
{
    public class Image
    {
        public required int Id { get; set; }
        public required int Year { get; set; }
        public string Extension { get; set; } = "";
        public string Name => Year + "/" + Id + Extension;

        public ImageDto ToImageDto()
        {
            return new ImageDto { Id = Id, Year = Year };
        }


    }
}
