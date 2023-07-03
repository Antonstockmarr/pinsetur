using stockmarrdk_api.Common;
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

        public Trip? PatchTrip(Trip trip)
        {
            Trip? oldTrip = _tripRepository.GetTripFromYear(trip.Year);
            if (oldTrip == null)
            {
                return null;
            }
            if (trip.Location != null)
            {
                oldTrip.Location = trip.Location;
            }
            if (trip.Description != null)
            {
                oldTrip.Description = trip.Description;
            }
            if (trip.Address != null)
            {
                oldTrip.Address = trip.Address;
            }
            if (trip.LocationImageId != null)
            {
                oldTrip.LocationImageId = trip.LocationImageId;
            }
            if (trip.CoverImageId != null)
            {
                oldTrip.CoverImageId = trip.CoverImageId;
            }
            _tripRepository.UpdateTrip(oldTrip);
            return oldTrip;
        }

        public void PostTrip(Trip trip)
        {
            if (GetTrip(trip.Year) != null)
            {
                throw new AlreadyExistsException($"Trip for year {trip.Year} already exists");
            }
            else
            {
                _tripRepository.CreateTrip(trip);
            }
        }
    }
}

