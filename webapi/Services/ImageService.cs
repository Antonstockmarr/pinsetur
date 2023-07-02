using Microsoft.AspNetCore.Http;
using stockmarrdk_api.Common;
using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;
using stockmarrdk_api.Repository;
using System.Linq;

namespace stockmarrdk_api.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        private readonly string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public List<Image> GetAllImages()
        {
            return _imageRepository.GetAllImages();
        }

        public List<Image> GetAllImagesFromYear(int year)
        {
            return _imageRepository.GetAllImages().FindAll(image => image.Year == year);
        }

        public async Task<ImageData?> GetImageDataFromId(int id)
        {
            Image? image = _imageRepository.GetImageById(id);
            if (image is null)
            {
                return null;
            }
            else
            {
                return await _imageRepository.GetImageDataFromImage(image);
            }
        }

        public Image? GetImageFromId(int id)
        {
            return _imageRepository.GetImageById(id);
        }

        public List<Image> GetAllCovers()
        {
            return _imageRepository.GetAllImages().FindAll(image => image.IsCover);
        }

        public Image? GetCoverFromYear(int year)
        {
            return GetAllCovers().FirstOrDefault(image => image.Year == year);
        }

        public async Task UploadImage(ImageUploadDto image)
        {
            int id = Math.Abs(Guid.NewGuid().GetHashCode());
            string extension = Path.GetExtension(image.File.FileName);

            string lowerExtention = extension.ToLowerInvariant();

            if (string.IsNullOrEmpty(lowerExtention) || !permittedExtensions.Contains(lowerExtention))
            {
                throw new BadRequestException("File type is not supported");
            }

            if (image.File.Length == 0)
            {
                throw new BadRequestException("File is empty");
            }

            using var fileStream = image.File.OpenReadStream();
            byte[] bytes = new byte[image.File.Length];
            fileStream.Read(bytes, 0, (int)image.File.Length);

            ImageData newImageData = new()
            { Id = id, Content = bytes, ContentType = image.File.ContentType };

            Image newImage = new() { Id = id, Extension = extension, IsCover = false, Year = image.Year };

            await _imageRepository.UploadImageData(newImageData, newImage);
            _imageRepository.UploadImage(newImage);
        }

    }
}
