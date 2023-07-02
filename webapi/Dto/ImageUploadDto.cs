namespace stockmarrdk_api.Dto
{
    public class ImageUploadDto
    {
        public int Year { get; set; }
        public required IFormFile File { get; set; }
    }
}
