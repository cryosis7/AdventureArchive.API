using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.Interfaces;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Factories;

public class LandmarkFactory : ILandmarkFactory
{
    public IDocLandmark CreateDocLandmark(string docId, string name, Location location, DocLandmarkType landmarkType, string? description = null)
    {
        return landmarkType switch
        {
            DocLandmarkType.Hut => new HutLandmark(docId, name, location, description),
            DocLandmarkType.Campsite => new CampsiteLandmark(docId, name, location, description),
            _ => throw new ArgumentOutOfRangeException(nameof(landmarkType), landmarkType, "Unsupported landmark type")
        };
    }

    public IDocLandmark CreateDocLandmark(string docId, string name, double latitude, double longitude, DocLandmarkType landmarkType, string? description = null)
    {
        var location = new Location(latitude, longitude);
        return CreateDocLandmark(docId, name, location, landmarkType, description);
    }
}