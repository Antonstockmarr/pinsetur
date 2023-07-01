using Azure;
using Azure.Data.Tables;
using stockmarrdk_api.Models;

namespace stockmarrdk_api.TableEntities
{
    public class TripEntity : ITableEntity
    {
        public int Year { get; set; }
        public string Location { get; set; } = "";
        public string Address { get; set; } = "";
        public string Description { get; set; } = "";
        public int LocationImageId { get; set; }

        // ITableEntity Properties
        public virtual string PartitionKey { get; set; } = "Trip";
        public virtual string RowKey { get => Year.ToString(); set => Year = int.Parse(value); }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public TripEntity() { }

        public TripEntity(Trip trip)
        {
            Year = trip.Year;
            Location = trip.Location;
            Address = trip.Address;
            Description = Description;
            LocationImageId = trip.LocationImageId;
        }

        public Trip ToTrip()
        {
            return new Trip { Year = Year, Location = Location, Address = Address, Description = Description, LocationImageId = LocationImageId };
        }
    }
}
