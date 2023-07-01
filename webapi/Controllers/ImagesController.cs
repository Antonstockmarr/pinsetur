using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<List<ImageDto>>> GetImages([FromQuery] int? year, [FromQuery] bool? onlyCovers)
        {
            List<Image>? images;
            if (onlyCovers is not null && (bool)onlyCovers)
            {
                if (year is not null)
                {
                    Image? cover = await _imageService.GetCoverFromYear((int)year);
                    if (cover is not null)
                    {
                        images = new List<Image>() { cover };                    
                    }
                    else
                    {
                        images = new List<Image>();
                    }
                }
                else
                {
                    images = await _imageService.GetAllCovers();
                }
            }
            if (year is not null)
            {
                images = await _imageService.GetAllImagesFromYear((int)year);
            }
            else
            {
                images = await _imageService.GetAllImages();
            }

            // Returns an empty array if no files are present at the storage container
            return StatusCode(StatusCodes.Status200OK, images.Select(image => image.ToImageDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Image? image = await _imageService.GetImageFromId(id);
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
    }
}