using stockmarrdk_api.Models;

namespace stockmarrdk_api.Services
{
    public interface IImageService
    {
        public Task<List<Image>> GetAllImages();

        public Task<List<Image>> GetAllImagesFromYear(int year);

        public Task<Image?> GetImageFromId(int id);

        public Task<ImageData?> GetImageDataFromId(int id);

        public Task<List<Image>> GetAllCovers();

        public Task<Image?> GetCoverFromYear(int year);
    }
}
