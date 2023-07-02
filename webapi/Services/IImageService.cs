using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;

namespace stockmarrdk_api.Services
{
    public interface IImageService
    {
        public List<Image> GetAllImages();

        public List<Image> GetAllImagesFromYear(int year);

        public Image? GetImageFromId(int id);

        public Task<ImageData?> GetImageDataFromId(int id);

        public List<Image> GetAllCovers();

        public Image? GetCoverFromYear(int year);

        public Task UploadImage(ImageUploadDto image);
    }
}
