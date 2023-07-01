using stockmarrdk_api.Models;
using stockmarrdk_api.Repository;

namespace stockmarrdk_api.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public Task<List<Trip>> GetAllTrips()
        {
            return Task.FromResult(_tripRepository.GetAllTrips());
        }

        public Task<Trip?> GetTrip(int year)
        {
            return Task.FromResult(_tripRepository.GetTripFromYear(year));
        }
    }
}

