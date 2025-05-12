using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.Visit;

public class HutVisit(ILandmark landmark, DateRange visitTime) : IVisit
{
    public ILandmark Landmark { get; } = landmark;
    public DateRange VisitPeriod { get; set; } = visitTime;
}