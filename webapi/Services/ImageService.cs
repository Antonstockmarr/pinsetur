using stockmarrdk_api.Models;
using stockmarrdk_api.Repository;

namespace stockmarrdk_api.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<List<Image>> GetAllImages()
        {
            return await _imageRepository.GetAllImages();
        }

        public async Task<List<Image>> GetAllImagesFromYear(int year)
        {
            return (await _imageRepository.GetAllImages()).FindAll(image => image.Year == year);
        }

        public async Task<ImageData?> GetImageDataFromId(int id)
        {
            Image? image = await _imageRepository.GetImageById(id);
            if (image is null)
            {
                return null;
            }
            else
            {
                return await _imageRepository.GetImageDataFromImage(image);
            }
        }

        public async Task<Image?> GetImageFromId(int id)
        {
            return await _imageRepository.GetImageById(id);
        }

        public async Task<List<Image>> GetAllCovers()
        {
            return (await _imageRepository.GetAllImages()).FindAll(image => image.IsCover);
        }

        public async Task<Image?> GetCoverFromYear(int year)
        {
            return (await GetAllCovers()).FirstOrDefault(image => image.Year == year);
        }
    }
}
