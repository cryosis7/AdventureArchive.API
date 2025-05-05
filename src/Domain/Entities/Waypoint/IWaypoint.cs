using AdventureArchive.Api.Domain.Entities.Visit;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.Waypoint;

public interface IWaypoint
{
    public string? Id { get; set; }
    public string Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? Notes { get; set; }
    public IVisitType VisitType { get; set; }

    public DateTime ArrivalTime => VisitDuration.Start;
    public DateRange VisitDuration { get; set; }
    
    public Location Location { get; set; }
    
    public Region? Region { get; set; }
}