using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.Visit;

public interface IVisitType
{
    string Name { get; }
    bool RequiresDuration { get; }
}

public class PassThroughVisitType : IVisitType
{
    public string Name => "Pass Through";
    public bool RequiresDuration => false;
}

public class StayVisitType : IVisitType
{
    public string Name => "Stay";
    public bool RequiresDuration => true;
}