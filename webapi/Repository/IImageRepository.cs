using stockmarrdk_api.Models;

namespace stockmarrdk_api.Repository
{
    public interface IImageRepository
    {
        List<Image> GetAllImages();

        Image? GetImageById(int id);

        Task<ImageData?> GetImageDataFromImage(Image image);

        Task UploadImageData(ImageData imageData, Image image);

        void UploadImage(Image image);
    }
}