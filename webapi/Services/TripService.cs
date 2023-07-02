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

        public List<Trip> GetAllTrips()
        {
            return _tripRepository.GetAllTrips();
        }

        public Trip? GetTrip(int year)
        {
            return _tripRepository.GetTripFromYear(year);
        }
    }
}

