using stockmarrdk_api.Models;

namespace stockmarrdk_api.Services
{
    public interface ITripService
    {
        Trip? DeleteTrip(int year);
        List<Trip> GetAllTrips();
        Trip? GetTrip(int year);
        Trip? PatchTrip(Trip trip);
        void PostTrip(Trip trip);
    }
}
