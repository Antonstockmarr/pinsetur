using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pinsetur.Webapi.Common;
using Pinsetur.Webapi.Dto;
using Pinsetur.Webapi.Models;
using Pinsetur.Webapi.Services;
using System.Security.Claims;

namespace Pinsetur.Webapi.Controllers
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
        [Authorize("Users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ImageDto>))]
        public ActionResult GetImages([FromQuery] int? year, [FromQuery] bool? myImages)
        {
            List<Image> images = new List<Image>();
            if (year is not null)
            {
                if (myImages ?? false)
                {
                    Claim? nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (nameIdentifier is not null)
                    {
                        images = _imageService.GetAllImagesUploadedByUser((int)year, nameIdentifier.Value);
                    }

                }
                else
                {
                    images = _imageService.GetAllImagesFromYear((int)year);
                }
            }
            else
            {
                images = _imageService.GetAllImages();
            }

            // Returns an empty array if no files are present at the storage container
            return StatusCode(StatusCodes.Status200OK, images.Select(image => image.ToImageDto()));
        }

        [HttpGet("{id}")]
        [Authorize("Users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [Authorize("Users")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [Authorize("Users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> UploadAsync([FromForm] ImageUploadDto image)
        {
            Claim? nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifier is null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            try
            {
                Image newImage = await _imageService.UploadImage(image, nameIdentifier.Value);
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

        [HttpPatch("{id}")]
        [Authorize("Admins")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult PatchImage([FromForm] ImageDto image)
        {
            try
            {
                Image? updatedImage = _imageService.PatchImage(image);
                if (updatedImage == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, updatedImage);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Delete(int id)
        {
            Claim? nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier);
            Image? image = _imageService.GetImageFromId(id);
            if (image == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }
            bool isOwner = nameIdentifier != null && image.UploadedBy != null && image.UploadedBy.Equals(nameIdentifier.Value);
            bool isAdmin = User.FindAll(ClaimTypes.Role).Any(role => role.Value.Equals(UserRole.Admin.ToString()));
            if (!isAdmin && !isOwner)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            try
            {
                Image? deletedImage = await _imageService.DeleteImageFromId(id);
                if (deletedImage == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, deletedImage.ToImageDto());
                }
            }
            catch (Exception ex)    
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("thumbnails")]
        [Authorize("Admins")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GenerateThumbnails()
        {
            try
            {
                await _imageService.GenerateThumbnails();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}