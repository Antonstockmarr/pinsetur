using Azure;
using Azure.Data.Tables;
using stockmarrdk_api.Models;

namespace stockmarrdk_api.TableEntities
{
    public class TripEntity : ITableEntity
    {
        public int Year { get => int.Parse(RowKey); set => RowKey = value.ToString(); }
        public string Location { get; set; } = "";
        public string Address { get; set; } = "";
        public string Description { get; set; } = "";
        public int? LocationImageId { get; set; }
        public int? CoverImageId { get; set; }

        // ITableEntity Properties
        public virtual string PartitionKey { get; set; } = "Trip";
        public virtual string RowKey { get; set; } = "";
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public TripEntity() { }

        public TripEntity(Trip trip)
        {
            RowKey = trip.Year.ToString();
            Location = trip.Location ?? "";
            Address = trip.Address ?? "";
            Description = trip.Description ?? "";
            LocationImageId = trip.LocationImageId;
            CoverImageId = trip.CoverImageId;
        }

        public Trip ToTrip()
        {
            return new Trip { Year = Year, Location = Location, Address = Address, Description = Description, LocationImageId = LocationImageId, CoverImageId = CoverImageId };
        }
    }
}
