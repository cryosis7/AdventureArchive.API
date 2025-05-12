using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Extensions;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.Visit;

public class HutVisit: IVisit
{
    public ILandmark Landmark { get; }
    public DateRange VisitPeriod { get; set; }

    private HutVisit(ILandmark landmark, DateRange visitTime)
    {
        Landmark = landmark;
        VisitPeriod = visitTime;
    }
    
    public static HutVisit Create(HutLandmark landmark, DateRange visitTime)
    {
        var visit = new HutVisit(landmark, visitTime);
        visit.Validate();
        return visit;
    }
    
    // TODO: It would be nice to send a hut to the api to see if it's all working...
}