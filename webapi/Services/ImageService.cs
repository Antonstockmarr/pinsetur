using stockmarrdk_api.Common;
using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;
using stockmarrdk_api.Repository;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using _Drawing = System.Drawing;

namespace stockmarrdk_api.Services
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageDataRepository _imageDataRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IUserRepository _userRepository;

        private readonly string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };
        private const int thumbnailWidth = 400;
        private const int thumbnailHeight = 400;

        public ImageService(IImageRepository imageRepository, IImageDataRepository imageDataRepository, ITripRepository tripRepository, IUserRepository userRepository)
        {
            _imageRepository = imageRepository;
            _imageDataRepository = imageDataRepository;
            _tripRepository = tripRepository;
            _userRepository = userRepository;

        }

        public async Task<Image?> DeleteImageFromId(int id)
        {
            Image? image = _imageRepository.DeleteImageById(id);
            if (image != null)
            {
                await _imageDataRepository.DeleteImageDataByImage(image.Name);
                await _imageDataRepository.DeleteImageDataByImage(image.ThumbName);

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
            if (image.UploadedBy != null && !_userRepository.UserNameExists(image.UploadedBy))
            {
                return null;
            }

            oldImage.Year = image.Year;
            if (image.Description != null)
            {
                oldImage.Description = image.Description;
            }
            if (image.UploadedBy != null)
            {
                oldImage.UploadedBy = image.UploadedBy;
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

            byte[] thumbnailBytes = GenerateThumbnail(bytes);
            Image newImage = new() { Id = id, Extension = extension, Year = image.Year, Description = image.Description, UploadedBy = uploadedByUsername };
            
            await _imageDataRepository.UploadImageData(bytes, newImage.Name);
            await _imageDataRepository.UploadImageData(thumbnailBytes, newImage.ThumbName);

            _imageRepository.UploadImage(newImage);

            return newImage;
        }

        public async Task GenerateThumbnails()
        {
            var images = _imageRepository.GetAllImages();
            foreach (var image in images)
            {
                var imageData = await _imageDataRepository.GetImageDataFromImage(image);

                if (imageData != null)
                {
                    await _imageDataRepository.DeleteImageDataByImage(image.ThumbName);

                    byte[] thumbnailBytes = GenerateThumbnail(imageData.Content);
                    await _imageDataRepository.UploadImageData(thumbnailBytes, image.ThumbName);
                }
            }
        }

        private static byte[] GenerateThumbnail(byte[] bytes)
        {
            using _Drawing.Image image = _Drawing.Image.FromStream(new MemoryStream(bytes));
            var height = image.Height;
            var width = image.Width;

            var ratio = (double)height / width;
            _Drawing.Bitmap newImage = new(thumbnailWidth, thumbnailHeight);
            using (_Drawing.Graphics gr = _Drawing.Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;

                var fullRectangle = new _Drawing.Rectangle(0, 0, thumbnailWidth, thumbnailHeight);
                // Crop image to make it 1:1
                var croppedRectangle = ratio > 1 ?
                    // x = 0, y = (h - h/2)/r, w = w, h = h/r
                    new _Drawing.Rectangle(0, (int)Math.Round((height - height / 2) / ratio), width, (int)Math.Round(height / (ratio)))
                    // x = w - w*r, y = 0, w = w*r, h = h
                    : new _Drawing.Rectangle((int)Math.Round((width - width * ratio) / 2), 0, (int)Math.Round(width * ratio), height);
                gr.DrawImage(image, fullRectangle, croppedRectangle, _Drawing.GraphicsUnit.Pixel);
            }

            using MemoryStream ms = new();
            newImage.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
}
