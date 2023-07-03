using Azure.Data.Tables;
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

        public void CreateTrip(Trip trip)
        {
            _tripTableClient.AddEntity(new TripEntity(trip));
        }

        public Trip? DeleteTrip(int year)
        {
            Trip? trip = GetTripFromYear(year);
            _tripTableClient.DeleteEntity(partitionKey: "Trip", rowKey: year.ToString());
            return trip;
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

        public void UpdateTrip(Trip trip)
        {
            _tripTableClient.UpdateEntity(entity: new TripEntity(trip), ifMatch: Azure.ETag.All, mode: TableUpdateMode.Replace);
        }
    }
}
