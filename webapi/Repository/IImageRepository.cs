using Pinsetur.Webapi.Models;

namespace Pinsetur.Webapi.Repository
{
    public interface IImageRepository
    {
        List<Image> GetAllImages();
        Image? GetImageById(int id);
        void UploadImage(Image image);
        Image? DeleteImageById(int id);
        void UpdateImage(Image image);
    }
}