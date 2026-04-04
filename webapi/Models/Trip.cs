using Pinsetur.Webapi.Common;
using System.ComponentModel.DataAnnotations;

namespace Pinsetur.Webapi.Models
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
        public string StartDate => DateHelper.WhitSunday(Year).AddDays(-2).ToString("yyyy-MM-dd");
        public string EndDate => DateHelper.WhitSunday(Year).AddDays(1).ToString("yyyy-MM-dd");
    }
}
