using AdventureArchive.Api.Domain.Entities.Visit;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.Waypoint;

public abstract class BaseWaypoint : IWaypoint
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? Notes { get; set; }
    public required IVisitType VisitType { get; set; }
    public required DateRange VisitDuration { get; set; }
    public required Location Location { get; set; }
    public Region? Region { get; set; }

    protected BaseWaypoint(
        IVisitType visitType,
        DateTime arrivalTime,
        DateTime? departureTime,
        TimeSpan? duration)
    {
        VisitType = visitType;
        if (visitType.RequiresDuration)
        {
            if (duration != null)
            {
                VisitDuration = new DateRange(arrivalTime, duration.Value);
            } else if (departureTime != null)
            {
                VisitDuration = new DateRange(arrivalTime, departureTime.Value);
            }
            else
            {
                throw new ArgumentNullException(nameof(departureTime), "Departure time or duration is required for stay visit type.");
            }
        }
        else
        {
            VisitDuration = new DateRange(arrivalTime, arrivalTime);
        }
    }
}