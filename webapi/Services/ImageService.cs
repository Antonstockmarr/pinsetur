using stockmarrdk_api.Common;
using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;
using stockmarrdk_api.Repository;

namespace stockmarrdk_api.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageDataRepository _imageDataRepository;
        private readonly ITripRepository _tripRepository;

        private readonly string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };

        public ImageService(IImageRepository imageRepository, IImageDataRepository imageDataRepository, ITripRepository tripRepository)
        {
            _imageRepository = imageRepository;
            _imageDataRepository = imageDataRepository;
            _tripRepository = tripRepository;
        }

        public async Task<Image?> DeleteImageFromId(int id)
        {
            Image? image = _imageRepository.DeleteImageById(id);
            if (image != null)
            {
                await _imageDataRepository.DeleteImageDataByImage(image);

                // Update references
                List<Trip> trips = _tripRepository.GetAllTrips();
                trips.ForEach(trip =>
                {
                    if (trip.CoverImageId == id)
                    {
                        trip.CoverImageId = null;
                        _tripRepository.UpdateTrip(trip);
                    }
                    if (trip.LocationImageId == id)
                    {
                        trip.LocationImageId = null;
                        _tripRepository.UpdateTrip(trip);
                    }
                });
            }
            return image;
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
                return await _imageDataRepository.GetImageDataFromImage(image);
            }
        }

        public Image? GetImageFromId(int id)
        {
            return _imageRepository.GetImageById(id);
        }

        public Image? PatchImage(ImageDto image)
        {
            Image? oldImage = _imageRepository.GetImageById(image.Id);
            if (oldImage == null)
            {
                return null;
            }

            oldImage.Year = image.Year;
            if (image.Description != null)
            {
                oldImage.Description = image.Description;
            }
            _imageRepository.UpdateImage(oldImage);
            return oldImage;
        }

        public async Task<Image> UploadImage(ImageUploadDto image, string uploadedByUsername)
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

            Image newImage = new() { Id = id, Extension = extension, Year = image.Year, Description = image.Description, UploadedBy = uploadedByUsername };

            await _imageDataRepository.UploadImageData(newImageData, newImage);
            _imageRepository.UploadImage(newImage);

            return newImage;
        }
    }
}
