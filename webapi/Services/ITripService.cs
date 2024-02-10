using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;

namespace stockmarrdk_api.Services
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
