namespace stockmarrdk_api.Dto
{
    public class ImageDto
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Uri { get; set; } = "";
        public string? Description { get; set; }
        public string? UploadedBy { get; set; }
    }
}
