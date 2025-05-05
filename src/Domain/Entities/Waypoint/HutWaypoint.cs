using AdventureArchive.Api.Domain.Entities.Visit;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.ValueObjects;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi.Models.Hut;

namespace AdventureArchive.Api.Domain.Entities.Waypoint;

public class HutWaypoint : BaseWaypoint
{
    public HutWaypoint(HutDto hutDto,
        IVisitType visitType,
        DateTime arrivalTime,
        DateTime? departureTime,
        TimeSpan? duration) : base(visitType, arrivalTime, departureTime, duration)
    {
        Id = hutDto.AssetId.ToString();
        Name = hutDto.Name;
        Location = Location.CreateLocation(hutDto.Lat, hutDto.Lon);
        Region = Enum.TryParse<RegionEnum>(hutDto.Region, out var region) ? region : null;
    }
}