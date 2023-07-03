using stockmarrdk_api.Models;

namespace stockmarrdk_api.Repository
{
    public interface IImageDataRepository
    {
        Task DeleteImageDataByImage(Image image);
        Task<ImageData?> GetImageDataFromImage(Image image);
        Task UploadImageData(ImageData imageData, Image image);
    }
}
