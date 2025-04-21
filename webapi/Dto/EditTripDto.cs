using Pinsetur.Webapi.Common;
using System.ComponentModel.DataAnnotations;

namespace Pinsetur.Webapi.Dto
{
    public class EditTripDto
    {
        public required int Year { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Location { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Address { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Description { get; set; }
        public int? LocationImageId { get; set; }
        public int? CoverImageId { get; set; }

    }
}
