namespace stockmarrdk_api.Models
{
    public class Trip
    {
        public int Year { get; set; }
        public string Location { get; set; } = "";
        public string Address { get; set; } = "";
        public string Description { get; set; } = "";
        public int LocationImageId { get; set; }
    }
}
