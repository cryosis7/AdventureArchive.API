using AdventureArchive.Api.Domain.Entities.Visit;
using AdventureArchive.Api.Domain.Entities.Waypoint;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.Trip;

public interface ITrip
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<IWaypoint> Waypoints { get; set; }
    public DateRange TripDateRange { get; set; }

    public IEnumerable<IWaypoint> GetChronologicalWaypoints()
    {
        return Waypoints.OrderBy(w => w.VisitDuration.Start);
    }
    
    public IEnumerable<IWaypoint> GetWaypointsForDay(DateOnly date)
    {
        return Waypoints
            .Where(waypoint => waypoint.VisitDuration.ContainsDay(date))
            .OrderBy(waypoint => waypoint.VisitDuration.Start);
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