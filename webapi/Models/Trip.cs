using System.ComponentModel.DataAnnotations;

namespace stockmarrdk_api.Models
{
    public class Trip
    {
        public required int Year { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Location { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Address { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Description { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int? LocationImageId { get; set; }
    }
}
