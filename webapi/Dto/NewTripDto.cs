using stockmarrdk_api.Common;
using System.ComponentModel.DataAnnotations;

namespace stockmarrdk_api.Dto
{
    public class NewTripDto
    {
        public required int Year { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Location { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Address { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Description { get; set; }
    }
}
