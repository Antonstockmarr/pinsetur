using Azure.Data.Tables;
using Azure.Storage.Blobs;
using stockmarrdk_api.Models;
using stockmarrdk_api.TableEntities;

namespace stockmarrdk_api.Repository
{
    public class TripRepository : ITripRepository
    {
        private readonly TableClient _tripTableClient;

        public TripRepository(IConfiguration configuration, TableServiceClient tableServiceClient)
        {
            string? storageTableName = configuration.GetValue<string>("TripsTableName");

            _tripTableClient = tableServiceClient.GetTableClient(storageTableName);
        }
        public List<Trip> GetAllTrips()
        {
            return _tripTableClient.Query<TripEntity>().Select(tripEntity => tripEntity.ToTrip()).OrderByDescending(trip => trip.Year).ToList();
        }

        public Trip? GetTripFromYear(int year)
        {
            TripEntity? tripEntity = _tripTableClient.Query<TripEntity>(tripEntity => tripEntity.PartitionKey == "Trip" && tripEntity.RowKey == year.ToString()).SingleOrDefault();
            if (tripEntity is null)
                return null;
            else
            {
                return (Trip?)tripEntity.ToTrip();
            }
        }
    }
}
