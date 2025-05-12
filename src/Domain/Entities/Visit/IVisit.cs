using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.Visit;

public interface IVisit
{
    ILandmark Landmark { get; }
    
    DateRange VisitPeriod { get; private protected set; }
    
    public void SetVisitPeriod(DateTime arrivalTime, DateTime departureTime)
    {
        VisitPeriod = new DateRange(arrivalTime, departureTime);
    }

    public void SetVisitPeriod(DateTime arrivalTime, TimeSpan duration)
    {
        VisitPeriod = new DateRange(arrivalTime, arrivalTime.Add(duration));
    }

    public void SetVisitPeriod(DateRange visitPeriod)
    {
        VisitPeriod = visitPeriod;
    }

    public void SetVisitTime(DateTime visitTime)
    {
        VisitPeriod = new DateRange(visitTime, visitTime);
    }

    public void UpdateArrivalTime(DateTime arrivalTime)
    {
        VisitPeriod = new DateRange(arrivalTime, VisitPeriod.End);
    }
    
    public void UpdateDepartureTime(DateTime departureTime)
    {
        VisitPeriod = new DateRange(VisitPeriod.Start, departureTime);
    }
}