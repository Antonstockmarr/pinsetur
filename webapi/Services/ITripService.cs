using stockmarrdk_api.Models;

namespace stockmarrdk_api.Services
{
    public interface ITripService
    {
        public List<Trip> GetAllTrips();
        public Trip? GetTrip(int year);
        public Trip? PatchTrip(Trip trip);
        public void PostTrip(Trip trip);
    }
}
