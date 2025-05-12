using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.Landmark;

public interface ILandmark
{
    Guid Id { get; }
    string Name { get; }
    string? Description { get; }
    Location Location { get; }
    public LandmarkType LandmarkType { get; }
}