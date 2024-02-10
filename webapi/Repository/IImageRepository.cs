using stockmarrdk_api.Models;

namespace stockmarrdk_api.Repository
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