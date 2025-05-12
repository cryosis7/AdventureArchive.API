using AdventureArchive.Api.Domain.Enums;

namespace AdventureArchive.Api.Domain.Entities.Landmark;

public interface IDocLandmark : ILandmark
{
    public string DocId { get; } 
    public DocLandmarkType LandmarkType { get; }
}