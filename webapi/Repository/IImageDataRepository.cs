using Pinsetur.Webapi.Models;

namespace Pinsetur.Webapi.Repository
{
    public interface IImageDataRepository
    {
        Task DeleteImageDataByImage(string path);
        Task<ImageData?> GetImageDataFromImage(Image image);
        Task UploadImageData(byte[] data, string path);
    }
}
