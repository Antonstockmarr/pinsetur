using stockmarrdk_api.Models;

namespace stockmarrdk_api.Repository
{
    public interface ITripRepository
    {
        public List<Trip> GetAllTrips();

        public Trip? GetTripFromYear(int year);
    }
}
