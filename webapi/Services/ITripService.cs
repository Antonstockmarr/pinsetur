using stockmarrdk_api.Models;

namespace stockmarrdk_api.Services
{
    public interface ITripService
    {
        public Task<List<Trip>> GetAllTrips();

        public Task<Trip?> GetTrip(int year);
    }
}
