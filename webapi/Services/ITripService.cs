using Pinsetur.Webapi.Dto;
using Pinsetur.Webapi.Models;

namespace Pinsetur.Webapi.Services
{
    public interface ITripService
    {
        Trip? DeleteTrip(int year);
        List<Trip> GetAllTrips();
        Trip? GetTrip(int year);
        Trip? PatchTrip(EditTripDto trip);
        Trip PostTrip(NewTripDto newTrip);
    }
}
