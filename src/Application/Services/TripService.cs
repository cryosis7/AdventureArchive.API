using AdventureArchive.Api.Domain.Entities.Trip;
using AdventureArchive.Api.Domain.Entities.Visit;
using AdventureArchive.Api.Domain.Interfaces;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Application.Services;

public class TripService(ITripProvider tripRepository) : ITripService
{
    private readonly ITripProvider _tripRepository =
        tripRepository ?? throw new ArgumentNullException(nameof(tripRepository));

    public async Task<Guid> CreateTripAsync(string name, DateTime startDate, DateTime? endDate, TimeSpan? duration)
    {
        ITrip newTrip;
        if (duration.HasValue)
        {
            newTrip = new SimpleTrip
            {
                Name = name,
                TripDateRange = new DateRange(startDate, duration.Value),
                Waypoints = new List<IVisit>()
            };
        }
        else
        {
            newTrip = new SimpleTrip
            {
                Name = name,
                TripDateRange = new DateRange(startDate, endDate ?? startDate),
                Waypoints = new List<IVisit>()
            };
        }
        await _tripRepository.AddAsync(newTrip);

        return newTrip.Id;
    }
}