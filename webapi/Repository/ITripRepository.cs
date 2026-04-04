using Pinsetur.Webapi.Models;

namespace Pinsetur.Webapi.Repository
{
    public interface ITripRepository
    {
        List<Trip> GetAllTrips();
        Trip? GetTripFromYear(int year);
        void CreateTrip(Trip trip);
        void UpdateTrip(Trip trip);
        Trip? DeleteTrip(int year);
    }
}
