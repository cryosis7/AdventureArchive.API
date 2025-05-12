using AdventureArchive.Api.Domain.Entities.Landmark;
using AdventureArchive.Api.Domain.Enums;
using AdventureArchive.Api.Domain.ValueObjects;

namespace AdventureArchive.Api.Domain.Interfaces;

public interface ILandmarkFactory
{
    IDocLandmark CreateDocLandmark(string docId, string name, Location location, DocLandmarkType landmarkType, string? description = null);
    
    IDocLandmark CreateDocLandmark(string docId, string name, double latitude, double longitude, DocLandmarkType landmarkType, string? description = null);
}