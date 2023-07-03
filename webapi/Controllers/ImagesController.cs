using Microsoft.AspNetCore.Mvc;
using stockmarrdk_api.Common;
using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;
using stockmarrdk_api.Services;

namespace stockmarrdk_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet()]
        public ActionResult GetImages([FromQuery] int? year)
        {
            List<Image>? images;
            if (year is not null)
            {
                images = _imageService.GetAllImagesFromYear((int)year);
            }
            else
            {
                images = _imageService.GetAllImages();
            }

            // Returns an empty array if no files are present at the storage container
            return StatusCode(StatusCodes.Status200OK, images.Select(image => image.ToImageDto()));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Image? image = _imageService.GetImageFromId(id);
            if (image is null)
            {
                // Was not, return error message to client
                return StatusCode(StatusCodes.Status404NotFound, $"Image with id {id} was not found.");
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, image.ToImageDto());
            }
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(int id)
        {
            ImageData? imageData = await _imageService.GetImageDataFromId(id);

            // Check if file was found
            if (imageData is null)
            {
                // Was not, return error message to client
                return StatusCode(StatusCodes.Status404NotFound, $"Image with id {id} was not found.");
            }
            else
            {
                return File(imageData.Content, imageData.ContentType, imageData.Id.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync([FromForm] ImageUploadDto image)
        {
            try
            {
                Image newImage = await _imageService.UploadImage(image);
                return StatusCode(StatusCodes.Status200OK, newImage.ToImageDto());
            }
            catch (BadRequestException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}