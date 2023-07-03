using stockmarrdk_api.Models;

namespace stockmarrdk_api.Repository
{
    public interface ITripRepository
    {
        List<Trip> GetAllTrips();

        Trip? GetTripFromYear(int year);

        void CreateTrip(Trip trip);
        void UpdateTrip(Trip trip);
    }
}
