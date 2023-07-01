using stockmarrdk_api.Models;

namespace stockmarrdk_api.Repository
{
    public interface IImageRepository
    {
        Task<List<Image>> GetAllImages();

        Task<Image?> GetImageById(int id);

        Task<ImageData?> GetImageDataFromImage(Image image);
    }
}