using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Extensions;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.VisitTime;

public interface IVisitTime : IValidatableEntity
{
    public DateRange VisitPeriod { get; }
    public DateTime ArrivalTime => VisitPeriod.Start;
    public VisitTypeEnum VisitType => VisitPeriod.Start == VisitPeriod.End ? VisitTypeEnum.PASS_THROUGH : VisitTypeEnum.STAY;
    
    void SetVisitPeriod(DateTime arrivalTime, DateTime departureTime);
    void SetVisitPeriod(DateTime arrivalTime, TimeSpan duration);
    void SetVisitPeriod(DateRange visitPeriod);
    void SetVisitTime(DateTime visitTime);
    void UpdateArrivalTime(DateTime arrivalTime);
    void UpdateDepartureTime(DateTime departureTime);
}