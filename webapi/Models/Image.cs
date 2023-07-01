using stockmarrdk_api.Dto;

namespace stockmarrdk_api.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Extension { get; set; } = "";
        public string Name => Year + "/" + Id + "." + Extension;
        public bool IsCover { get; set; }

        public ImageDto ToImageDto()
        {
            return new ImageDto { Id = Id, Year = Year };
        }


    }
}
