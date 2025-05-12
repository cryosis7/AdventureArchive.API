using AdventureArchive.Api.Domain.Entities.Visit;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.Trip;

public class SimpleTrip : ITrip
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required IEnumerable<IVisit> Waypoints { get; set; }
    public required DateRange TripDateRange { get; set; }
}