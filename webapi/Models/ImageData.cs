namespace stockmarrdk_api.Models
{
    public class ImageData
    {
        public int Id { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    
        public ImageData()
        {
            ContentType = "";
            Content = Array.Empty<byte>();
        }
    }

}
