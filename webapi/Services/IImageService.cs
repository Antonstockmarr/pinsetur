using Pinsetur.Webapi.Dto;
using Pinsetur.Webapi.Models;

namespace Pinsetur.Webapi.Services
{
    public interface IImageService
    {
        List<Image> GetAllImages();
        List<Image> GetAllImagesFromYear(int year);
        List<Image> GetAllImagesUploadedByUser(int year, string username);
        Image? GetImageFromId(int id);
        Task<ImageData?> GetImageDataFromId(int id);
        Task<Image> UploadImage(ImageUploadDto image, string username);
        Task<Image?> DeleteImageFromId(int id);
        Image? PatchImage(ImageDto image);
        Task GenerateThumbnails();
    }
}
