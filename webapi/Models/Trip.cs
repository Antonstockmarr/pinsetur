using stockmarrdk_api.Common;
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
        public int? LocationImageId { get; set; }
        public int? CoverImageId { get; set; }
        public string StartDate => DateHelper.WhitSunday(Year).AddDays(-2).ToShortDateString();
        public string EndDate => DateHelper.WhitSunday(Year).AddDays(1).ToShortDateString();
    }
}
