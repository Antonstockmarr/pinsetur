using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;

namespace stockmarrdk_api.Services
{
    public interface IImageService
    {
        List<Image> GetAllImages();
        List<Image> GetAllImagesFromYear(int year);
        Image? GetImageFromId(int id);
        Task<ImageData?> GetImageDataFromId(int id);
        Task<Image> UploadImage(ImageUploadDto image, string username);
        Task<Image?> DeleteImageFromId(int id);
        Image? PatchImage(ImageDto image);
    }
}
