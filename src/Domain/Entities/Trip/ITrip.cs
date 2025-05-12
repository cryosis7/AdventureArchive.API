using AdventureArchive.Api.Domain.Entities.Visit;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.Trip;

public interface ITrip
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<IVisit> Waypoints { get; set; }
    public DateRange TripDateRange { get; set; }

    public IEnumerable<IVisit> GetChronologicalWaypoints()
    {
        return Waypoints.OrderBy(w => w.VisitPeriod.Start);
    }
    
    public IEnumerable<IVisit> GetWaypointsForDay(DateOnly date)
    {
        return Waypoints
            .Where(waypoint => waypoint.VisitPeriod.ContainsDay(date))
            .OrderBy(waypoint => waypoint.VisitPeriod.Start);
    }

    public T ConvertTrip<T>() where T : ITrip, new()
    {
        return new T
        {
            Name = Name,
            Id = Id,
            Description = Description,
            Waypoints = Waypoints,
            TripDateRange = TripDateRange
        };
    }
}