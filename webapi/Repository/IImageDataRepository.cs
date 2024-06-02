using stockmarrdk_api.Models;

namespace stockmarrdk_api.Repository
{
    public interface IImageDataRepository
    {
        Task DeleteImageDataByImage(string path);
        Task<ImageData?> GetImageDataFromImage(Image image);
        Task UploadImageData(byte[] data, string path);
    }
}
