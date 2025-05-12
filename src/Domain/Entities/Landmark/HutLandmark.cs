using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Entities.Landmark;

public class HutLandmark : ILandmark
{
    public Guid Id { get; }
    public string DocId { get; }
    public string Name { get; }
    public string? Description { get; }
    public Location Location { get; }

    public LandmarkType LandmarkType => LandmarkType.Hut;

    internal HutLandmark(string docId, string name, Location location, string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(docId);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(location);

        Id = Guid.NewGuid();
        DocId = docId;
        Name = name;
        Location = location;
        Description = description;
    }
}