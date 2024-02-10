using stockmarrdk_api.Common;
using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;
using stockmarrdk_api.Repository;
using System.ComponentModel.DataAnnotations;

namespace stockmarrdk_api.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;

        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public Trip? DeleteTrip(int year)
        {
            return _tripRepository.DeleteTrip(year);
        }

        public List<Trip> GetAllTrips()
        {
            return _tripRepository.GetAllTrips();
        }

        public Trip? GetTrip(int year)
        {
            return _tripRepository.GetTripFromYear(year);
        }

        public Trip? PatchTrip(EditTripDto trip)
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

        public Trip PostTrip(NewTripDto newTrip)
        {
            if (GetTrip(newTrip.Year) != null)
            {
                throw new AlreadyExistsException($"Trip for year {newTrip.Year} already exists");
            }
            else
            {
                var trip = new Trip
                {
                    Year = newTrip.Year,
                    Location = newTrip.Location,
                    Address = newTrip.Address,
                    Description = newTrip.Description
                };
                _tripRepository.CreateTrip(trip);
                return trip;
            }
        }
    }
}

